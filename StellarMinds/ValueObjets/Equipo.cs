using StellarMinds.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using StellarMinds.InterfacesDominio;

namespace StellarMinds.ValueObjets
{
    public class Equipo : IValidable
    {
        public Montura Montura { get; private set; }
        public Telescopio Telescopio { get; private set; }
        public Ocular Ocular { get; private set; }
        public Camara Camara { get; private set; }


        public void Validar()
        {
            throw new NotImplementedException();
        }
    }
}
