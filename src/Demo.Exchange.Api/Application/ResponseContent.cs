namespace Demo.Exchange.Application
{
    using System;
    using System.Text.Json;

    public readonly struct ResponseContent
    {
        private static Type _inputType;
        public static string _contentAsJsonString;

        private ResponseContent(byte[] contentData) => Value = contentData;

        public byte[] Value { get; }

        public static ResponseContent Create<TContent>(TContent contentData) where TContent : struct
        {
            _inputType = contentData.GetType();

            var contentAsByte = JsonSerializer.SerializeToUtf8Bytes(contentData);
            return new ResponseContent(contentAsByte);
        }

        /// <summary>
        /// JSON String
        /// </summary>
        /// <param name="contentData"></param>
        /// <returns></returns>
        public static ResponseContent Create(string contentData, Type inputType)
        {
            _inputType = inputType;
            _contentAsJsonString = contentData;

            var value = JsonSerializer.Deserialize(contentData, inputType);

            var contentAsByte = JsonSerializer.SerializeToUtf8Bytes(value);
            return new ResponseContent(contentAsByte);
        }

        /// <summary>
        /// JSON String
        /// </summary>
        /// <param name="contentData"></param>
        /// <returns></returns>
        public static ResponseContent Create(byte[] contentData, Type inputType)
        {
            _inputType = inputType;
            return new ResponseContent(contentData);
        }

        public TContent GetRaw<TContent>()
        {
            if (Value is null)
                throw new ArgumentException(nameof(Value));

            var reader = new Utf8JsonReader(new ReadOnlySpan<byte>(Value));
            return JsonSerializer.Deserialize<TContent>(ref reader);
        }

        public object GetRaw(Type inputType)
        {
            if (Value is null)
                throw new ArgumentException(nameof(Value));

            var reader = new Utf8JsonReader(new ReadOnlySpan<byte>(Value));
            return JsonSerializer.Deserialize(ref reader, inputType);
        }

        public string ValueAsJsonString
        {
            get
            {
                if (string.IsNullOrEmpty(_contentAsJsonString))
                {
                    var jsonData = GetRaw(_inputType);
                    _contentAsJsonString = JsonSerializer.Serialize(jsonData, _inputType);
                }

                return _contentAsJsonString;
            }
        }
    }
}