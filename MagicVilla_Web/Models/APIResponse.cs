﻿using System.Net;

namespace MagicVilla_Web.Models
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }


        // Bydefault the IsSuccess is set to true but if any error comes in future we need to change it to false.
        public bool IsSuccess { get; set; } = true;

        public List<string> ErrorMessages { get; set; }

        public object Result { get; set; }
    }
}
