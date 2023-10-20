using RedeNeuralRascunho.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedeNeuralRascunho.Controladores
{
    public class Controlador
    {
        List<Entrada> entradas = new List<Entrada>();  





        public Controlador() 
        {
            entradas.Add(new Entrada() { 
                Nome = "Direita",
                
            });


        }

        public void Executa()
        {

        }
    }
}
