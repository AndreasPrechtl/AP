using AP.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace AP.Reflection;


[Flags]
public enum MemberPathQueryOptions
{
    Properties = 0x1,
    Fields = 0x2,
    All = Properties | Fields
}

public static class ObjectExtensions
{
    public static MemberPath Path<TContext, TResult>(Expression<Func<TContext, TResult>> memberExpression, MemberPathQueryOptions options = MemberPathQueryOptions.Properties)
    {
        ArgumentNullException.ThrowIfNull(memberExpression);

        var excludeFields = (options & MemberPathQueryOptions.Fields) != MemberPathQueryOptions.Fields;
        var excludeProperties = (options & MemberPathQueryOptions.Properties) != MemberPathQueryOptions.Properties;

        var memberNames = new List<string>();

        var body = (MemberExpression)memberExpression.Body;
        while (body is not null)
        {
            var memberType = body.Member.MemberType;

            if (memberType is not (MemberTypes.Property or MemberTypes.Field))
                throw new InvalidOperationException("use fields or properties only"); ;

            if (excludeFields && memberType is MemberTypes.Field)
                throw new InvalidOperationException("field in path is not allowed");

            if (excludeProperties && memberType is MemberTypes.Property)
                throw new InvalidOperationException("property in path is not allowed");

            memberNames.Insert(0, body.Member.Name);

            body = body.Expression as MemberExpression;
        }

        return new MemberPath(memberNames);
    }
}

// todo: record / struct?
[Serializable]
public sealed class MemberPath : IComparable
{
    public sealed class SegmentList : AP.Collections.ReadOnly.ReadOnlyList<string>
    {
        internal readonly string _value;
        
        private SegmentList(IEnumerable<string> segments, string value)
            : base(segments)
        {
            _value = value;
        }

        internal static SegmentList Create(IEnumerable<string> segments)
        {
            ArgumentNullException.ThrowIfNull(segments);

            StringBuilder sb = new();
            AP.Collections.List<string> list = [];

            foreach (string s in segments)
            {
                string[] split = s.Split('.');

                if (split.Length > 0)
                {
                    foreach (string s0 in split)
                    {
                        string t = s0.Trim();
                        if (!t.IsNullOrWhiteSpace())
                        {
                            list.Add(t);
                            sb.Append(t);
                            sb.Append('.');
                        }
                    }
                }
                else
                {
                    string t = s.Trim();
                    if (!t.IsNullOrWhiteSpace())
                    {
                        list.Add(t);
                        sb.Append(t);
                        sb.Append('.');
                    }
                }
            }

            if (list.Count > 0)
            {
                // remove the last '.'
                sb.Remove(sb.Length - 1, 1);
            }

            return new SegmentList(list, sb.ToString());
        }

        public new SegmentList Clone() => new(this.ToReadOnlyList(), _value);
    }

    private readonly string _name;
    private readonly SegmentList _segments;
    private static readonly MemberPath s_empty = new(string.Empty);

    public static MemberPath Empty => s_empty;

    public SegmentList Segments => _segments;

    public MemberPath(string memberPath)
        : this(New.Enumerable<string>(memberPath))
    { }

    public MemberPath(IEnumerable<string> segments)
    {
        SegmentList s = SegmentList.Create(segments);
        _segments = s;
        _name = s[^1];
    }

    /// <summary>
    /// Gets the member name.
    /// </summary>
    public string Name => _name;

    /// <summary>
    /// Gets the string representation.
    /// </summary>
    public string Value => _segments._value;

    /// <summary>
    /// Gets the total length of the string representation (including separators).
    /// </summary>
    public int Length => _segments._value.Length;

    public override int GetHashCode() => this.Value.GetHashCode();

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;

        if (obj == this)
            return true;

        if (obj is MemberPath path)
            return this.Value.Equals(path.Value, StringComparison.OrdinalIgnoreCase);

        return this.Value.Equals(obj.ToString(), StringComparison.OrdinalIgnoreCase);
    }
    
    public static implicit operator MemberPath?(string? path)
    {
        return path != null ? new MemberPath(path) : null;
    }

    public static implicit operator string?(MemberPath? path)
    {
        return path != null ? path.Value : null;
    }

    int IComparable.CompareTo(object? obj)
    {
        if (this.Equals(obj))
            return 0;
        else
            return _segments._value.CompareTo(obj);
    }
}
