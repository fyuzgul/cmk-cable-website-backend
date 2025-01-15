using System;
using System.Collections.Generic;

namespace CmkCable.Entities
{
    public class GetOffer
    {
        public string AdSoyad { get; set; }
        public string FirmaAdi { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }
        public string Unvan { get; set; }
        public string Ulke { get; set; }
        public string Kablolar { get; set; }
        public string Aciklama { get; set; }
        public string Lme { get; set; }
        public List<string> ParaBirimleri { get; set; } = new List<string>();
        public string TeslimSekli { get; set; }
        public string TeslimYeri { get; set; }
        public string OdemeSekli { get; set; }
        public string Ambalajlama { get; set; }
        public bool AcikRiza { get; set; }

        public override string ToString()
        {
            return $"AdSoyad: {AdSoyad}, FirmaAdi: {FirmaAdi}, Telefon: {Telefon}, Email: {Email}, " +
                   $"Unvan: {Unvan}, Ulke: {Ulke}, Kablolar: {Kablolar}, Aciklama: {Aciklama}, " +
                   $"Lme: {Lme}, ParaBirimleri: [{string.Join(", ", ParaBirimleri)}], TeslimSekli: {TeslimSekli}, " +
                   $"TeslimYeri: {TeslimYeri}, OdemeSekli: {OdemeSekli}, Ambalajlama: {Ambalajlama}, AcikRiza: {AcikRiza}";
        }
    }
}
