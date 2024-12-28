﻿using System;

namespace AP.ComponentModel.Validation;

public class ValidationEventArgs : EventArgs
{
    private readonly ValidationResult _result;
    public ValidationResult Result => _result;

    public ValidationEventArgs(ValidationResult result)
    {
        _result = result;
    }
}
