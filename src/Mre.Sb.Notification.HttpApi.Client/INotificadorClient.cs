namespace Mre.Sb.Notificacion.HttpApi
{
    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.14.7.0 (NJsonSchema v10.5.2.0 (Newtonsoft.Json v12.0.0.0))")]
    public partial interface INotificadorClient
    {
        /// <returns>Success</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        System.Threading.Tasks.Task<bool> NotificadorAsync(NotificacionMensajeDto body);

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Success</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        System.Threading.Tasks.Task<bool> NotificadorAsync(NotificacionMensajeDto body, System.Threading.CancellationToken cancellationToken);

        void SetAccessToken(string accessToken);

        void AddHeaders(string name, string value);
    }



    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.5.2.0 (Newtonsoft.Json v12.0.0.0)")]
    public partial class NotificacionMensajeDto
    {
        [System.Text.Json.Serialization.JsonPropertyName("codigo")]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [System.ComponentModel.DataAnnotations.StringLength(60)]
        public string Codigo { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("asunto")]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Asunto { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("destinatarios")]
        public string Destinatarios { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("model")]
        public System.Collections.Generic.IDictionary<string, object> Model { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.5.2.0 (Newtonsoft.Json v12.0.0.0)")]
    public partial class NotificacionResultadoDto
    {
        [System.Text.Json.Serialization.JsonPropertyName("estado")]
        public NotificacionEstado Estado { get; set; }


    }


    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.5.2.0 (Newtonsoft.Json v12.0.0.0)")]
    public enum NotificacionEstado
    {
        _0 = 0,

        _1 = 1,

        _2 = 2,

        _3 = 3,

    }
}
