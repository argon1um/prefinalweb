﻿using System;
using System.Collections.Generic;

namespace AHRestAPI.Models;

public partial class Service
{
    public int ServiceId { get; set; }

    public int? ServiceCategid { get; set; }

    public string? ServiceName { get; set; }

    public string? ServiceDescription { get; set; }

    public double? ServicePrice { get; set; }

    public string? ServiceImage { get; set; }

    public virtual Servicecategory? ServiceCateg { get; set; }
}
