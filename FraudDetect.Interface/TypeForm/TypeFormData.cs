namespace FraudDetect.Interface.TypeForm
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class TypeFormData
    {
        [JsonProperty(PropertyName = "event_id")]
        public string EventId { get; set; }

        [JsonProperty(PropertyName = "event_type")]
        public string EventType { get; set; }

        [JsonProperty(PropertyName = "form_response")]
        public TypeFormFormResponse FormResponse { get; set; }
    }

    public class TypeFormFormResponse
    {
        [JsonProperty(PropertyName = "form_id")]
        public string FormId { get; set; }

        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }

        [JsonProperty(PropertyName = "landed_at")]
        public DateTime LandedAt { get; set; }

        [JsonProperty(PropertyName = "submitted_at")]
        public DateTime SubmittedAt { get; set; }

        [JsonProperty(PropertyName = "hidden")]
        public Dictionary<string, string> HiddenFields { get; set; }

        [JsonProperty(PropertyName = "definition")]
        public TypeFormDefinition Definition { get; set; }

        [JsonProperty(PropertyName = "answers")]
        public List<TypeFormAnswer> Answers { get; set; }
    }

    public class TypeFormDefinition
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "fields")]
        public List<TypeFormField> Fields { get; set; }
    }

    public class TypeFormField
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string FiledType { get; set; }

        [JsonProperty(PropertyName = "ref")]
        public string Ref { get; set; }
    }

    public class TypeFormAnswerConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            if (reader == null || reader.TokenType == JsonToken.Null) return null;

            var obj = JObject.Load(reader);

            //if (obj != null && obj.ChildrenTokens != null && obj.ChildrenTokens.Count > 1 && obj.ChildrenTokens[0].Name == "type")
            if (obj != null && obj.Count > 1 && obj.ContainsKey("type"))
            {
                var propertyType = obj["type"].Value<string>();

                var result = new TypeFormAnswer();

                if (propertyType.Equals("email", StringComparison.InvariantCultureIgnoreCase) ||
                    propertyType.Equals("number", StringComparison.InvariantCultureIgnoreCase) ||
                    propertyType.Equals("text", StringComparison.InvariantCultureIgnoreCase) ||
                    propertyType.Equals("phone_number", StringComparison.InvariantCultureIgnoreCase))
                {
                    result.FieldValue = obj[propertyType].Value<string>();
                }

                result.Field = obj["field"].ToObject<TypeFormField>();

                return result;
            }

            return null;
        }

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }
    }

    [JsonConverter(typeof(TypeFormAnswerConverter))]
    public class TypeFormAnswer
    {
        public string FieldName { get; set; }

        public string FieldValue { get; set; }

        public TypeFormField Field { get; set; }
    }
}