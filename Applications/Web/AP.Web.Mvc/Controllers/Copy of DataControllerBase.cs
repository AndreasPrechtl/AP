using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AP.Data;
using AP.Web.Mvc;
using AP.Web.Mvc.Routing;
using AP.Web.Mvc.Security;
using System.Web.UI;

namespace AP.Web.Mvc.Controllers
{
    [HandleError(View="Error")]
    public abstract class DataControllerBase<TDataObject> : AP.Web.Mvc.Controllers.ControllerBase
        where TDataObject : class, IDataObject
    {
        protected virtual IOrderedQueryable<TDataObject> BaseQuery { get; set; }
        protected virtual IDataRepository<TDataObject> Repository { get; private set; }

        protected override void Initialize()
        {
            base.Initialize();

            this.Repository = Application.Repositories.GetRepositoryProvider<TDataObject>().CreateDataRepository<TDataObject>();
            
            this.BaseQuery = from e in this.Repository
                             orderby e.Id descending
                             select e;
        }

        protected virtual TDataObject EncodeDataObject(TDataObject dataObject)
        {
            return dataObject;
        }

        public virtual ActionResult List(int page, int size)
        {
            PagedViewModel<TDataObject> model = this.CreatePagedViewModel(page, size, true);
            
            if (model.Current == null || model.Current.Count() == 0)
                return View(Actions.Empty);
            
            if (model.Count < page)
                throw new HttpRequestValidationException();

            if (size == 1)
                return this.RedirectToAction(Actions.Details, new { id = model.Current.First().Id });

            return base.View(model);
        }

        public virtual ActionResult Details(long id)
        {
            ViewModel<TDataObject> model = this.CreateViewModel(new long?(id), false);
            if (model.Current == null)
                throw new NotSupportedException(string.Format("data with the id {0} was not found", id));

            return base.View(model);
        }

        [Authorize]
        //[RequireHttps]
        public virtual ActionResult Create(long? id)
        {
            return this.View(this.CreateViewModel(id, true));
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize, ValidateInput(false)]
        //[RequireHttps]
        public virtual ActionResult Create([Bind(Exclude = "Id, RowGuid, CreatedBy, DateCreated, ModifiedBy, DateModified")] TDataObject dataObject)
        {
            return this.RedirectToAction(Actions.Details, new { id = this.Repository.Insert(this.EncodeDataObject(dataObject)).Id });
        }

        [Authorize]
        //[RequireHttps]
        public virtual ActionResult Edit(long id)
        {
            return this.View(this.CreateViewModel(new long?(id), false));
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize, ValidateInput(false)]
        //[RequireHttps]        
        public virtual ActionResult Edit([Bind(Exclude = "CreatedBy, DateCreated, ModifiedBy, DateModified")] TDataObject dataObject)
        {
            return this.RedirectToAction(Actions.Details, new { id = this.Repository.Update(this.EncodeDataObject(dataObject)).Id });
        }

        [Authorize, HttpPost, ValidateAntiForgeryToken]
        //[RequireHttps]        
        public virtual ActionResult Delete(long id)
        {
            this.Repository.Delete<TDataObject>(p => p.Id == id);

            return base.RedirectToAction(Actions.List);
        }

        //[Authorize, HttpPost, ValidateAntiForgeryToken, RequireHttps]
        //public virtual ActionResult Delete(TDataObject dataObject)
        //{
        //    this.Repository.Delete(dataObject);

        //    return base.RedirectToAction(Actions.List);
        //}
                
        [ChildActionOnly]
        public virtual PartialViewResult Widget()
        {
            return base.PartialView(this.CreatePagedViewModel(0, Application.Settings.MaximumWidgetEntries, true));
        }

        protected virtual PagedViewModel<TDataObject> CreatePagedViewModel(int page, int size, bool defaultIfEmpty = true)
        {
            if (page < 0)
                throw new ArgumentException("page");
            
            if (size < 1)
                throw new ArgumentException("size");
            
            object first = null;
            object previous = null;
            object next = null;
            object last = null;
            
            IQueryable<TDataObject> baseQuery = this.BaseQuery;
            
            int count = baseQuery.Count<TDataObject>();
            int lastPageIndex = (int)Math.Ceiling((double)count / size) - 1;
            
            if (page < 0 || ((page > lastPageIndex) && (lastPageIndex > -1)))
                throw new ArgumentException("page");
            
            ReadOnlyCollection<TDataObject> current = new ReadOnlyCollection<TDataObject>(baseQuery.Skip<TDataObject>((size * page)).Take<TDataObject>(size).ToList<TDataObject>());
            
            if (count > 1 && current != null)
            {
                if (page > 0)
                {
                    first = PagedViewModelHelper.CreateRouteValues(0, size);
                    previous = PagedViewModelHelper.CreateRouteValues(page - 1, size);
                }
                if (page < lastPageIndex)
                {
                    next = PagedViewModelHelper.CreateRouteValues(page + 1, size);
                    last = PagedViewModelHelper.CreateRouteValues(lastPageIndex, size);
                }
            }
            if (current == null && defaultIfEmpty)
                current = new ReadOnlyCollection<TDataObject>(new List<TDataObject>());
            
            else if (!defaultIfEmpty && current != null && current.Count == 0)
                current = null;            

            return new PagedViewModel<TDataObject>(current, page, count, first, previous, next, last, size);
        }
        
        protected virtual ViewModel<TDataObject> CreateViewModel(long? id, bool defaultIfEmpty = false)
        {
            object first = null;
            object previous = null;
            object next = null;
            object last = null;

            IOrderedQueryable<TDataObject> baseQuery = this.BaseQuery;
            TDataObject current = baseQuery.FirstOrDefault(p => p.Id == id);
            
            int count = baseQuery.Count<TDataObject>();
            int currentIndex = 0;            

            if (id.HasValue && (current != null))
            {
                currentIndex = baseQuery.Count(p => p.Id < id);
                if (currentIndex > 0)
                {
                    previous = first = ViewModelWrapperHelper.CreateRouteValues(baseQuery.Min(p => p.Id));
                    if (currentIndex > 1)
                        previous = ViewModelWrapperHelper.CreateRouteValues(baseQuery.Select(p => p.Id).Where(p => p < id).Max(p => (long?)p));                    
                }

                int countAfter = baseQuery.Count(p => p.Id > id);

                if (countAfter > 0)
                {
                    next = last = ViewModelWrapperHelper.CreateRouteValues(baseQuery.Max(p => (long?)p.Id));
                    if (countAfter > 1)                   
                        next = ViewModelWrapperHelper.CreateRouteValues(baseQuery.Select(p => p.Id).Where(p => p > id).Min(p => (long?)p));
                }
            }
            else if (count > 0)
            {
                current = Activator.CreateInstance<TDataObject>();

                first = baseQuery.Min(p => (long?)p.Id);
                previous = baseQuery.Max(p => (long?)p.Id);

                //first = this.CreateRouteValues(Queryable.Min<TDataObject, long?>(baseQuery, (Expression<Func<TDataObject, long?>>) (p => p.Id)));
                //previous = this.CreateRouteValues(Queryable.Max<TDataObject, long?>(baseQuery, (Expression<Func<TDataObject, long?>>) (p => p.Id)));
                
                currentIndex = count;
            }

            if (current == null && defaultIfEmpty)
                current = Activator.CreateInstance<TDataObject>();
            
            return new ViewModel<TDataObject>(current, currentIndex, count, first, previous, next, last);
        }
                
        protected override void Dispose(bool disposing)
        {
            if (this.Repository != null)
            {
                this.Repository.Dispose();
                this.Repository = null;
            }
        }

    }
}

