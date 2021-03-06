﻿using System;

namespace CopyWords.Parsers
{
    public class PageParserException : Exception
    {
        public PageParserException()
        {
        }

        public PageParserException(string message)
            : base(message)
        {
        }

        public PageParserException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected PageParserException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
