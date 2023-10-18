using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace CobrinhaNeural.Entidades
{
    public class Cobra
    {
        public List<TextBlock> textBlocks { get; set; }
        public int posX { get; set; }
        public int posY { get; set; }
        public (int X, int Y) posCrescer { get; set; }
        public List<(int X, int Y)> posCorpo { get; set; }
        public int frente { get; set; } = 0;
        public string[] direcoesTradutor { get; set; }

        public Cobra()
        {
            textBlocks = new List<TextBlock>();
            posCorpo = new List<(int X, int Y)>();
            iniciaDirecoesTradutor();
        }

        public void moverCobra(Grid gridPai, int valor, string eixo)
        {
            int contadorPartes = 0;
            int posXAnterior = posX;
            int posYAnterior = posY;

            foreach (TextBlock block in textBlocks)
            {
                if (contadorPartes == 0)
                {
                    gridPai.Children.Remove(block);
                    moverTextBlock(block, valor, eixo, posX, posY);
                    gridPai.Children.Add(block);


                    switch (eixo)
                    {
                        case "X":
                            posX += valor;
                            break;
                        case "Y":
                            posY += valor;
                            break;
                    }
                }
                else
                {
                    int alvoX = posXAnterior;
                    int alvoY = posYAnterior;
                    posXAnterior = posCorpo[contadorPartes - 1].X;
                    posYAnterior = posCorpo[contadorPartes - 1].Y;

                    gridPai.Children.Remove(block);
                    moverTextBlock(block, 0, "", alvoX, alvoY);
                    posCorpo[contadorPartes - 1] = (alvoX, alvoY);
                    gridPai.Children.Add(block);

                }

                posCrescer = (posXAnterior, posYAnterior);
                contadorPartes++;
            }
        }

        private void moverTextBlock(TextBlock block, int valor, string eixo, int posXPar, int posYPar)
        {
            switch (eixo)
            {
                case "X":
                    Grid.SetColumn(block, posXPar + valor);
                    break;
                case "Y":
                    Grid.SetRow(block, posYPar + valor);
                    break;
                default:
                    Grid.SetRow(block, posYPar + valor);
                    Grid.SetColumn(block, posXPar + valor);
                    break;
            }
        }

        public void virar(int lado)
        {
            //NEGATIVO = esquerda -- POSITIVO = direita
            if (lado > 0)
            {
                frente = lado + frente > 3 ? 0 : lado + frente;
            }
            else
            {
                frente = lado + frente < 0 ? 3 : lado + frente;
            }
        }

        public (int prevX, int prevY) retornaPrevisaoPasso(bool inverso = false)
        {
            int inverter = 1;
            int X = posX;
            int Y = posY;

            if (inverso)
                inverter = -1;

            switch (frente)
            {
                case 0:
                    X += -1 * inverter;
                    break;
                case 1:
                    Y += 1 * inverter;
                    break;
                case 2:
                    X += 1 * inverter;
                    break;
                case 3:
                    Y += -1 * inverter;
                    break;
            }

            return (X, Y);
        }

        void iniciaDirecoesTradutor()
        {
            direcoesTradutor = new string[4];
            direcoesTradutor[0] = "Esquerda";
            direcoesTradutor[1] = "Baixo";
            direcoesTradutor[2] = "Direita";
            direcoesTradutor[3] = "Cima";
        }
    }
}
