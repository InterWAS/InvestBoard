using System.ComponentModel.DataAnnotations;

namespace InvestBoardAPI.Data
{
    /// <summary>
    /// Entrada de evento de telemetria contendo informações sobre o acesso ao endpoint, tempo de resposta e registro de data e hora do evento.
    /// </summary>
    /// <remarks>
    /// Usada para registrar e analisar dados de telemetria para solicitações de endpoint, 
    /// incluindo quando o evento ocorreu, qual endpoint foi acessado e quanto tempo levou a resposta. 
    /// Cada instância identifica exclusivamente um evento de telemetria e pode ser usada para fins de monitoramento ou diagnóstico.
    /// A Data e Hora são armazenadas separadamente para facilitar consultas e análises baseadas em data ou hora específicas.
    /// </remarks>
    public class Telemetria
    {
        /// <summary>
        /// Identificador único da entrada do evento de telemetria
        /// </summary>
        [Key]
        [Display(Name = "ID")]
        public int Id { get; set; }

        /// <summary>
        /// Data da entrada do evento de telemetria
        /// </summary>
        [Required]
        [Display(Name = "Data")]
        public DateOnly Data { get; set; }

        /// <summary>
        /// Hora da entrada do evento de telemetria
        /// </summary>
        [Required]
        [Display(Name = "Hora")]
        public TimeOnly Hora { get; set; }

        /// <summary>
        /// URL do endpoint acessado
        /// </summary>
        [Required]
        [Display(Name = "Endpoint")]
        public string Endpoint { get; set; } = string.Empty;

        /// <summary>
        /// Tempo de resposta do endpoint em milissegundos
        /// </summary>
        [Required]
        [Display(Name = "Tempo de Resposta (ms)")]
        public long TempoResposta { get; set; }
    }
}
