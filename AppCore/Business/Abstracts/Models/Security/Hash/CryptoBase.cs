namespace AppCore.Business.Abstracts.Models.Security.Hash
{
    public abstract class CryptoBase
    {
        public byte[] Hash { get; set; } // hash'lenmiş veri
        public byte[] Salt { get; set; } // hash'lenmiş veriyi kuvvetlendirmek için
    }
}
