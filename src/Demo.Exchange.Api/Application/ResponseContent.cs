namespace Demo.Exchange.Application
{
    using System;
    using System.Text.Json;

    public readonly struct ResponseContent
    {
        private static Type _inputType;
        private static string _contentAsJsonString = string.Empty;

        private ResponseContent(byte[] contentData, Type inputType)
        {
            Value = contentData;
            _inputType = inputType;
        }

        public byte[] Value { get; }

        public static ResponseContent Create<TContent>(TContent contentData) where TContent : struct
        {
            var contentAsByte = JsonSerializer.SerializeToUtf8Bytes(contentData);
            return new ResponseContent(contentAsByte, contentData.GetType());
        }

        /// <summary>
        /// JSON String
        /// </summary>
        /// <param name="contentData"></param>
        /// <returns></returns>
        public static ResponseContent Create(string contentData, Type inputType)
        {
            _contentAsJsonString = contentData ?? throw new ArgumentNullException(nameof(contentData));

            var value = JsonSerializer.Deserialize(contentData, inputType);

            var contentAsByte = JsonSerializer.SerializeToUtf8Bytes(value);
            return new ResponseContent(contentAsByte, inputType);
        }

        /// <summary>
        /// JSON String
        /// </summary>
        /// <param name="contentData"></param>
        /// <returns></returns>
        public static ResponseContent Create(byte[] contentData, Type inputType)
        {
            if (contentData == null)
                throw new ArgumentNullException(nameof(contentData));

            if (inputType == null)
                throw new ArgumentNullException(nameof(inputType));

            return new ResponseContent(contentData, inputType);
        }

        public TContent GetRaw<TContent>() => (TContent)GetRaw(typeof(TContent));

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
                    _contentAsJsonString = JsonSerializer.Serialize(GetRaw(_inputType), _inputType);

                return _contentAsJsonString;
            }
        }
    }
}