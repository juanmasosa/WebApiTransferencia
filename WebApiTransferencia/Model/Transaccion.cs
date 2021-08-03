namespace WebApiTransferencia.Model

{
    public class Transaccion
    {   
        public int Id { get; set; }
        public int cuentaOrigen { get; set; }
        public int cuentaDestino { get; set; }
        public double montoTransferencia { get; set; }
    }
}
