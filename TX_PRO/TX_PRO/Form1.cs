using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TX_PRO
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int[,] matrizTipos = {
        {146,147,146,199,0},
        {147,147,0,199,0},
        {147,147,0,199,0},
        {147,147,0,199,0},
        {0,0,0,199,0},
        {0,0,0,199,0},
        {0,0,0,199,0},
        {0,0,0,199,0},
        {0,0,0,199,199}
    };


        MatrizEstados me = new MatrizEstados();
        TablaCaracteres tc = new TablaCaracteres();
        MatrizProducciones produ = new MatrizProducciones();
        MatrizPredictiva predi = new MatrizPredictiva();

        String desc = "", plres = "";
        int edofinal, apCuadruplo, apSimbolos, apConstantes;
        OpenFileDialog ventana_archivos = new OpenFileDialog();
        StreamReader leer;
        String texto, smb, cte, ctef;
        int apTexto, auxTP = 0, operador = 0, tipoConstante = 0, simocte = 0;
        char[] arrayTexto;

        Stack<int> ejecucion = new Stack<int>();
        Stack<int> operadores = new Stack<int>();
        Stack<int> operandos = new Stack<int>();
        Stack<int> tipos = new Stack<int>();
        Stack<int> saltos = new Stack<int>();
        Stack<int> temporales = new Stack<int>();

        int[,] cuadruplos = new int[200, 4];
        Object[,] simbolos = new Object[200, 5];
        Object[,] constantes = new Object[200, 3];

        public object InputBox { get; private set; }


        //Tabla validar palabra
        public Boolean Vreservadas(string pl)
        {
            switch (pl)
            {
                case "constructor":
                case "commit":
                case "if":
                case "end":
                case "while":
                case "endwhile":
                case "endif":
                case "do":
                case "dowhile":
                case "enddo":
                case "read":
                case "write":
                case "int":
                case "float":
                case "string":
                case "else":
                case "char": return true;
            }
            return false;
        }

        // Tabla de tokens
        public int Token(int edo)
        {
            switch (edo)
            {

                case 1: // se ajusta el token
                case 100:
                    if (Vreservadas(plres))
                    {
                        edofinal = PalabrasReservadas(plres);
                        if (edofinal >= 146 && edofinal <= 149)
                        {
                            auxTP = edofinal;
                        }
                        return edofinal;
                    }
                    desc = "\n100: Identificador";
                    return edofinal = 100;
                case 2: // se austa el token
                case 101:
                    if (Vreservadas(plres))
                    {
                        return PalabrasReservadas(plres);
                    }
                    desc = "\n101: Identificador";
                    return edofinal = 101;
                case 3: // se ajusta el token
                case 102:
                    desc = "\n 102:Constante numerica";
                    return edofinal = 102;
                case 5: //se ajusta el token
                case 103:
                    desc = "\n 103:Constante numerica (REALES)";
                    return edofinal = 103;
                case 8:// se ajusta el token
                case 104:
                    desc = "\n 104:Constante numerica (N.C)";
                    return edofinal = 104;
                case 105:
                    desc = "\n 105:Constante caracter";
                    return edofinal = 105;
                case 106:
                    desc = "\n 106:Amperson";
                    return edofinal = 106;
                case 107:
                    desc = "\n 107:Pleca";
                    return edofinal = 107;
                case 108:
                    desc = "\n 108:Negacion\n";
                    return edofinal = 108;
                case 109:
                    desc = "\n 109:Diferente de\n";
                    return edofinal = 109;
                case 110:
                    desc = "\n 110:Comparacion de equivalencia\n";
                    return edofinal = 110;
                case 111:
                    desc = "\n 111:Asignacion\n";
                    return edofinal = 111;
                case 112:
                    desc = "\n 112:Constante String\n";
                    return edofinal = 112;
                case 113:
                    desc = "\n 113:Mayor que\n";
                    return edofinal = 113;
                case 115:
                    desc = "\n 115:Mayor o igual que\n";
                    return edofinal = 115;
                case 116:
                    desc = "\n 116:Menor que\n";
                    return edofinal = 116;
                case 117:
                    desc = "\n 117:Menor o igual que\n";
                    return edofinal = 117;
                case 118:
                    desc = "\n 118:Parentesis que abre\n";
                    return edofinal = 118;
                case 119:
                    desc = "\n 119:Parentesis que cierra\n";
                    return edofinal = 119;
                case 120:
                    desc = "\n 120:Corchete que cierra\n";
                    return edofinal = 120;
                case 121:
                    desc = "\n 121:Corchete que abre\n";
                    return edofinal = 121;
                case 122:
                    desc = "\n 122:Llave que cierra\n";
                    return edofinal = 122;
                case 123:
                    desc = "\n 123:Llave que abre\n";
                    return edofinal = 123;
                case 124:
                    desc = "\n 124:Suma\n";
                    return edofinal = 124;
                case 125:
                    desc = "\n 125:Resta\n";
                    return edofinal = 125;
                case 126:
                    desc = "\n 126:Division\n";
                    return edofinal = 126;
                case 127:
                    desc = "\n 127:Multiplicacion\n";
                    return edofinal = 127;
                case 128:
                    desc = "\n 128:Porsentaje\n";
                    return edofinal = 128;
                case 18: //Se ajusta token
                case 129:
                    desc = "\n 139:Comentario\n";
                    return edofinal = 129;
                case 130:
                    desc = "\n 130:Coma\n";
                    return edofinal = 130;
                case 131:
                    desc = "\n 131:Punto y coma\n";
                    return edofinal = 131;
                case 132:
                    desc = "\n 132:Dos puntos\n";
                    return edofinal = 132;
                case 133:
                    desc = "\n 133:Punto\n";
                    return edofinal = 133;
            }
            return -1;
        }

        // Tabla de errores
        public int Error(int edo)
        {
            switch (edo)
            {
                case 500:
                    desc = "ERROR 500: Se espera digito";
                    return edofinal = 500;
                case 501:
                    desc = "ERROR 501: Se espera comilla simple de cierre";
                    return edofinal = 501;
                case 502:
                    desc = "ERROR 502: Se espera un amper";
                    return edofinal = 502;
                case 4: // se ajusta token
                case 503:
                    desc = "ERROR 503: Se espera digito (N.R)";
                    return edofinal = 503;
                case 7: // se ajusta el token
                case 504:
                    desc = "ERROR 504: Se espera digito (N.C)";
                    return edofinal = 504;
                case 6: // se ajusta el token
                case 505:
                    desc = "ERROR 505: Se espera signo (- o +) o digito";
                    return edofinal = 505;
                case 506:
                    desc = "ERROR 506: Caracter no reconocido por el lenguaje";
                    return edofinal = 506;
                case 507:
                    desc = "ERROR 507: Se espera una pleca";
                    return edofinal = 500;
            }
            return -1;
        }

        // Tabla  de palabras reservadas
        public int PalabrasReservadas(string pl)
        {
            switch (pl)
            {
                case "constructor":
                    desc = "CONSTRUCTOR";
                    return 134;
                case "commit":
                    desc = "135:COMMIT";
                    return 135;
                case "if":
                    desc = "136:IF";
                    return 136;
                case "end":
                    desc = "137:END";
                    return 137;
                case "while":
                    desc = "138:WHILE";
                    return 138;
                case "endwhile":
                    desc = "139:ENDWHILE";
                    return 139;
                case "endif":
                    desc = "140:ENDIF";
                    return 140;
                case "do":
                    desc = "141:DO";
                    return 141;
                case "dowhile":
                    desc = "142:DOWHILE";
                    return 142;
                case "enddo":
                    desc = "143:ENDDO";
                    return 143;
                case "read":
                    desc = "144:READ";
                    return 144;
                case "write":
                    desc = "145:WRITE";
                    return 145;
                case "int":
                    desc = "146:INT";
                    return 146;
                case "float":
                    desc = "147:FLOAT";
                    return 147;
                case "string":
                    desc = "148:STRING";
                    return 148;
                case "char":
                    desc = "149:CHAR";
                    return 149;
                case "else":
                    desc = "150:ELSE";
                    return 150;
            }
            return -1;
        }

        // Tabla match
        private int Match(int numero)
        {
            int mat = 0;
            switch (numero)
            {
                case 100: return 1013;
                case 101: return 1013;
                case 102: return 1043;
                case 103: return 1044;
                case 104: return 1045;
                case 105: return 1046;
                case 106: return 1029;
                case 107: return 1030;
                case 108: return 1031;
                case 109: return 1038;
                case 110: return 1037;
                case 111: return 1026;
                case 112: return 1047;
                case 113: return 1041;
                case 115: return 1042;
                case 116: return 1039;
                case 117: return 1040;
                case 118: return 1002;
                case 119: return 1003;
                case 120: return 1015;
                case 121: return 1014;
                case 122: return 1005;
                case 123: return 1004;
                case 124: return 1032;
                case 125: return 1033;
                case 126: return 1034;
                case 127: return 1035;
                case 128: return 1036;
                case 129: return 129;
                case 130: return 1016;
                case 131: return 1008;
                case 132: return 1017;
                case 133: return 133;
                case 134: return 1001;
                case 135: return 1006;
                case 136: return 1021;
                case 137: return 1007;
                case 138: return 1024;
                case 139: return 1025;
                case 140: return 1023;
                case 141: return 1018;
                case 142: return 1019;
                case 143: return 1020;
                case 144: return 1027;
                case 145: return 1028;
                case 146: return 1009;
                case 147: return 1010;
                case 148: return 1012;
                case 149: return 1011;
                case 150: return 1022;
            }
            return mat;
        }

        // Tabla de acciones 
        public void Accion(int tope)
        {
            switch (tope)
            {
                case 2001:
                    int ps1 = 0;
                    for (int i = 0; i < apSimbolos; i++)
                    {
                        if (smb == simbolos[i, 1].ToString())
                        {
                            ps1 = i;
                        }
                    }
                    int op1 = (int)simbolos[ps1, 0];
                    int tp1 = (int)simbolos[ps1, 2];
                    operandos.Push(op1);
                    tipos.Push(tp1);
                    simocte = 1;
                    break;
                case 2002:
                    operadores.Push(107);
                    break;
                case 2003:
                    operadores.Push(106);
                    break;
                case 2004:
                    operadores.Push(108);
                    break;
                case 2005:
                    operadores.Push(operador);
                    break;
                case 2006:
                    operadores.Push(operador);
                    break;
                case 2007:
                    int ps2 = -1;
                    for (int i = 0; i < apConstantes; i++)
                    {
                        if (ctef == constantes[i, 1].ToString())
                        {
                            ps2 = i;
                        }
                    }
                    if (ps2 == -1)
                    {
                        GeConstantes(apConstantes + 4000, ctef, tipoConstante);
                        ps2 = apConstantes - 1;
                    }
                    op1 = (int)constantes[ps2, 0];
                    tp1 = (int)constantes[ps2, 2];
                    operandos.Push(op1);
                    tipos.Push(tp1);
                    simocte = 2;
                    break;
                case 2010:
                    if (operadores.Count() > 0)
                    {
                        if (operadores.Last() == 126 || operadores.Last() == 127 || operadores.Last() == 128)
                        {
                            int ope = operadores.Pop();
                            int opa2 = operandos.Pop();
                            int opa1 = operandos.Pop();
                            int tip2 = tipos.Pop();
                            int tip1 = tipos.Pop();
                            int tpf = compTipos(tip1, tip2, ope);
                            if (tpf == 0)
                            {
                                MessageBox.Show("Error de tipos");
                                break;
                            }
                            else
                            {
                                int res = temporales.Pop();
                                GeCuadruplo(ope, opa1, opa2, res);
                                GeSimbolos(res, "temporal", tpf, null, null);
                                operandos.Push(res);
                                tipos.Push(tpf);
                                /*if (opa1 >= 3200 && opa1 <= 3230)
                                {
                                    temporales.Push(opa1);
                                }
                                if (opa2 >= 3200 && opa2 <= 3230)
                                {
                                    temporales.Push(opa2);
                                }*/
                            }
                        }
                    }
                    break;
                case 2011:
                    if (operadores.Count() > 0)
                    {
                        if (operadores.Last() == 124 || operadores.Last() == 125)
                        {
                            int ope = operadores.Pop();
                            int opa2 = operandos.Pop();
                            int opa1 = operandos.Pop();
                            int tip2 = tipos.Pop();
                            int tip1 = tipos.Pop();
                            int tpf = compTipos(tip1, tip2, ope);
                            if (tpf == 0)
                            {
                                MessageBox.Show("Error de tipos");
                                break;
                            }
                            else
                            {
                                int res = temporales.Pop();
                                GeCuadruplo(ope, opa1, opa2, res);
                                GeSimbolos(res, "temporal", tpf, null, null);
                                operandos.Push(res);
                                tipos.Push(tpf);
                                /*if (opa1 >= 3200 && opa1 <= 3230)
                                {
                                    temporales.Push(opa1);
                                }
                                if (opa2 >= 3200 && opa2 <= 3230)
                                {
                                    temporales.Push(opa2);
                                }*/
                            }
                        }
                    }
                    break;
                case 2012:
                    operadores.Push(operador);
                    break;
                case 2013:
                    if (operadores.Last() >= 113 || operadores.Last() <= 117)
                    {
                        int ope = operadores.Pop();
                        int opa2 = operandos.Pop();
                        int opa1 = operandos.Pop();
                        int tip2 = tipos.Pop();
                        int tip1 = tipos.Pop();
                        int tpf = compTipos(tip1, tip2, ope);
                        if (tpf == 0)
                        {
                            MessageBox.Show("Error de tipos");
                            break;
                        }
                        else
                        {
                            int res = temporales.Pop();
                            GeCuadruplo(ope, opa1, opa2, res);
                            GeSimbolos(res, "temporal", tpf, null, null);
                            operandos.Push(res);
                            tipos.Push(tpf);
                            /*if (opa1 >= 3200 && opa1 <= 3230)
                            {
                                temporales.Push(opa1);
                            }
                            if (opa2 >= 3200 && opa2 <= 3230)
                            {
                                temporales.Push(opa2);
                            }*/
                        }
                    }
                    break;
                case 2014:
                    int opnot = operadores.Pop();
                    int tipono = tipos.Pop();
                    if (opnot != 108 || tipono != 199)
                    {
                        MessageBox.Show("Error con not");
                        break;
                    }
                    else
                    {
                        int ope = operandos.Pop();
                        int res = temporales.Pop();
                        GeCuadruplo(108, ope, 0, res);
                        GeSimbolos(res, "temporal", 199, null, null);
                        operandos.Push(res);
                        tipos.Push(199);
                        /*if (ope >= 3200 && ope <= 3230)
                        {
                            temporales.Push(ope);
                        }*/
                    }
                    break;
                case 2015:
                    if (operadores.Count() > 0)
                    {
                        if (operadores.Last() == 106)
                        {
                            int ope = operadores.Pop();
                            int opa2 = operandos.Pop();
                            int opa1 = operandos.Pop();
                            int tip2 = tipos.Pop();
                            int tip1 = tipos.Pop();
                            int tpf = compTipos(tip1, tip2, ope);
                            if (tpf == 0)
                            {
                                MessageBox.Show("Error de tipos");
                                break;
                            }
                            else
                            {
                                int res = temporales.Pop();
                                GeCuadruplo(ope, opa1, opa2, res);
                                GeSimbolos(res, "temporal", tpf, null, null);
                                operandos.Push(res);
                                tipos.Push(tpf);
                                /*if (opa1 >= 3200 && opa1 <= 3230)
                                {
                                    temporales.Push(opa1);
                                }
                                if (opa2 >= 3200 && opa2 <= 3230)
                                {
                                    temporales.Push(opa2);
                                }*/
                            }
                        }
                    }
                    break;
                case 2016:
                    if (operadores.Count() > 0)
                    {
                        if (operadores.Last() == 107)
                        {
                            int ope = operadores.Pop();
                            int opa2 = operandos.Pop();
                            int opa1 = operandos.Pop();
                            int tip2 = tipos.Pop();
                            int tip1 = tipos.Pop();
                            int tpf = compTipos(tip1, tip2, ope);
                            if (tpf == 0)
                            {
                                MessageBox.Show("Error de tipos");
                                break;
                            }
                            else
                            {
                                int res = temporales.Pop();
                                GeCuadruplo(ope, opa1, opa2, res);
                                GeSimbolos(res, "temporal", tpf, null, null);
                                operandos.Push(res);
                                tipos.Push(tpf);
                                /*if (opa1 >= 3200 && opa1 <= 3230)
                                {
                                    temporales.Push(opa1);
                                }
                                if (opa2 >= 3200 && opa2 <= 3230)
                                {
                                    temporales.Push(opa2);
                                }*/
                            }
                        }
                    }
                    break;
                case 2017:
                    tipos.Push(auxTP);
                    break;
                case 2018:
                    GeSimbolos(apSimbolos + 3000, smb, 0, null, null);
                    operandos.Push(apSimbolos + 2999);
                    //MessageBox.Show("Simbolo generado");
                    break;
                case 2019:
                    int tipo = tipos.Pop();
                    while (operandos.Count() != 0)
                    {
                        int op = operandos.Pop();
                        //MessageBox.Show(op + " operador");
                        int pos = 0;
                        for (int i = 0; i < apSimbolos; i++)
                        {
                            //   MessageBox.Show(simbolos[i, 0] + " cuadruplo");
                            if (op == ((int)simbolos[i, 0]))
                            {
                                pos = i;
                            }
                        }
                        simbolos[pos, 2] = tipo;
                    }
                    break;
                case 2020:
                    int opread = -2;
                    for (int i = 0; i < apSimbolos; i++)
                    {
                        if (smb == (simbolos[i, 1].ToString()))
                        {
                            opread = (int)simbolos[i, 0];
                        }
                    }
                    if (opread == -2)
                    {
                        GeSimbolos(apSimbolos + 3000, smb, 0, null, null);
                        opread = apSimbolos + 2999;
                    }
                    GeCuadruplo(144, 0, 0, opread);
                    break;
                case 2021:
                    int opw = -2;
                    if (simocte == 1)
                    {
                        for (int i = 0; i < apSimbolos; i++)
                        {
                            if (smb == (simbolos[i, 1].ToString()))
                            {
                                opw = (int)simbolos[i, 0];
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < apConstantes; i++)
                        {
                            if (ctef == (constantes[i, 1].ToString()))
                            {
                                opw = (int)constantes[i, 0];
                            }
                        }
                    }
                    if (opw == -2)
                    {
                        GeConstantes(apConstantes + 4000, ctef, 0);
                        opw = apConstantes + 3999;
                    }
                    GeCuadruplo(145, 0, 0, opw);
                    break;
                case 2022:
                    int tipE = tipos.Pop();
                    int tipR = tipos.Pop();
                    if (tipE == tipR)
                    {
                        int expr = operandos.Pop();
                        int res = operandos.Pop();
                        GeCuadruplo(111, expr, 0, res);
                        /*if (expr >= 3200 && expr <= 3230)
                        {
                            temporales.Push(expr);
                        }*/
                    }
                    else
                    {
                        MessageBox.Show("Error de tipos " + tipE + " != " + tipR);
                        break;
                    }
                    break;
                case 2023:
                    saltos.Push(apCuadruplo);
                    break;
                case 2024:
                    int aux = tipos.Pop();
                    if (aux != 199)
                    {
                        MessageBox.Show("Error semantico");
                    }
                    else
                    {
                        int res = operandos.Pop();
                        // goto = 400
                        // gotoF = 401
                        // gotoV = 402
                        GeCuadruplo(401, res, 0, 0);
                        saltos.Push(apCuadruplo - 1);

                    }
                    break;
                case 2025:
                    int falso = saltos.Pop();
                    int retorno = saltos.Pop();

                    GeCuadruplo(400, 0, 0, retorno);

                    cuadruplos[falso, 3] = apCuadruplo;
                    break;
                case 2026:
                    saltos.Push(apCuadruplo);
                    break;
                case 2027:
                    int aux2 = tipos.Pop();
                    if (aux2 != 199)
                    {
                        MessageBox.Show("Error semantico");
                    }
                    else
                    {
                        int res = operandos.Pop();
                        // goto = 400
                        // gotoF = 401
                        // gotoV = 402
                        int s = saltos.Pop();
                        GeCuadruplo(402, res, 0, s);

                    }
                    break;
                case 2028:
                    int aux3 = tipos.Pop();
                    if (aux3 != 199)
                    {
                        MessageBox.Show("Error semantico");
                    }
                    else
                    {
                        int res = operandos.Pop();
                        // goto = 400
                        // gotoF = 401
                        // gotoV = 402
                        GeCuadruplo(401, res, 0, 0);
                        saltos.Push(apCuadruplo - 1);

                    }

                    break;
                case 2029:
                    GeCuadruplo(400, 0, 0, 0);
                    int fals = saltos.Pop();
                    cuadruplos[fals, 3] = apCuadruplo;
                    saltos.Push(apCuadruplo - 1);
                    break;
                case 2030:
                    int fin = saltos.Pop();
                    cuadruplos[fin, 3] = apCuadruplo;
                    break;
            }
        }

        private void btnEjecutar_Click(object sender, EventArgs e)
        {
            ejecucionProg();
            imprimirTablas();
        }

        // Metodo para generara cuadruplos
        public void GeCuadruplo(int cod, int op1, int op2, int res)
        {
            cuadruplos[apCuadruplo, 0] = cod;
            cuadruplos[apCuadruplo, 1] = op1;
            cuadruplos[apCuadruplo, 2] = op2;
            cuadruplos[apCuadruplo, 3] = res;
            apCuadruplo++;
        }

        // Metodo tabla de simbolos
        public void GeSimbolos(int dir, Object simbolo, int tipo, Object ram, Object ap)
        {
            simbolos[apSimbolos, 0] = dir;
            simbolos[apSimbolos, 1] = simbolo;
            simbolos[apSimbolos, 2] = tipo;
            simbolos[apSimbolos, 3] = ram;
            simbolos[apSimbolos, 4] = ap;
            apSimbolos++;
        }

        // Metodo para constantes
        public void GeConstantes(int cod, Object op1, int op2)
        {
            constantes[apConstantes, 0] = cod;
            constantes[apConstantes, 1] = op1;
            constantes[apConstantes, 2] = op2;
            apConstantes++;
        }

        // metodo imprime tablas
        public void imprimirTablas()
        {
            String tablaC = "", tablaS = "", tablaCo = "";

            tablaC += "\t-------Tabla de Cuadruplos---------\r\n";
            tablaC += "Num\tCod OP\t | OP 1\t | OP 2\t | Res";
            for (int i = 0; i < apCuadruplo; i++)
            {
                tablaC += "\r\n" + i + "\t" + cuadruplos[i, 0] + "\t| " + cuadruplos[i, 1] + "\t| " + cuadruplos[i, 2] + "\t| " + cuadruplos[i, 3];
            }

            txtBoxCuadruplos.Text = tablaC;

            tablaS += "\t------------Tabla de Simbolos-------------\r\n";
            tablaS += "Dir\t| Desc\t| Tipo\t| RAM\t| Ap";
            for (int i = 0; i < apSimbolos; i++)
            {
                tablaS += "\r\n" + simbolos[i, 0] + "\t| " + simbolos[i, 1] + "\t| " + simbolos[i, 2] + "\t| " + simbolos[i, 3] + "\t| " + simbolos[i, 4];
            }

            txtBoxSimbolos.Text = tablaS;

            tablaCo += "\t----------Tabla de Constantes---------\t";
            tablaCo += "Dir\t| Val\t| Tipo";
            for (int i = 0; i < apConstantes; i++)
            {
                tablaCo += "\r\n" + constantes[i, 0] + "\t| " + constantes[i, 1] + "\t| " + constantes[i, 2];
            }

            txtBoxConstantes.Text = tablaCo;
        }

        // metodo lexico
        public int Lexico()
        {
            int edo = 0, col, car = ' ';
            plres = "";
            cte = "";

            while (edo <= 19 && apTexto < texto.Length)
            {
                car = arrayTexto[apTexto];
                if (car == 36)
                {
                    apTexto++;
                    apTexto++;
                    return -1;
                }

                //MessageBox.Show(car+"");
                col = tc.Relacion(car);
                //MessageBox.Show("Columna: " + col + " Estado: " + edo);
                edo = me.Matriz(edo, col);
                // MessageBox.Show("final"+edo);


                if (edo == 1)
                {
                    char letra = Convert.ToChar(car);
                    plres += letra;
                }

                if (edo >= 3 && edo <= 11)
                {
                    char letra = Convert.ToChar(car);
                    cte += letra;
                    //MessageBox.Show("cte: " + cte);
                }

                if ((edo <= 19 || (edo >= 117 && edo <= 132) || edo == 105 || edo == 106 || edo == 107 || edo == 112 || edo == 115) && (apTexto < texto.Length))
                {
                    apTexto++;
                    car = arrayTexto[apTexto];
                }
            }

            while (car == 32 || car == 10 || car == 09)
            {
                apTexto++;
                car = arrayTexto[apTexto];
            }

            if ((edo >= 102 && edo <= 105) || edo == 112)
            {
                if (edo == 102) tipoConstante = 146;
                if (edo >= 103 && edo <= 104) tipoConstante = 147;
                if (edo == 105) tipoConstante = 149;
                if (edo == 112) tipoConstante = 148;
            }

            if ((edo >= 110 && edo <= 111) || (edo >= 113 && edo <= 117) || (edo >= 124 && edo <= 128))
            {
                operador = edo;
            }

            if (edo == 100 || edo == 101)
            {
                smb = plres;
                //MessageBox.Show("id: "+smb);
            }

            if ((edo >= 102 && edo <= 105) || edo == 112)
            {
                ctef = cte;
            }

            if (edo >= 100 && edo <= 133)
            {
                Token(edo);
                txtBoxLexico.Text += desc + "\n";
            }
            else
            {
                Error(edo);
                txtBoxLexico.Text += desc + "\n";
            }
            //MessageBox.Show(plres+"");
            return edofinal;
        }

        //metodo para comparar tipos de variables
        public int compTipos(int t1, int t2, int op)
        {
            int tipo = 0;
            int n1 = 0, n2 = 0;
            if (t1 == 146 && t2 == 146)
            {
                n1 = 0;
            }
            if (t1 == 146 && t2 == 147)
            {
                n1 = 1;
            }
            if (t1 == 147 && t2 == 146)
            {
                n1 = 2;
            }
            if (t1 == 147 && t2 == 147)
            {
                n1 = 3;
            }
            if (t1 == 149 && t2 == 149)
            {
                n1 = 4;
            }
            if (t1 == 149 && t2 == 148)
            {
                n1 = 5;
            }
            if (t1 == 148 && t2 == 149)
            {
                n1 = 6;
            }
            if (t1 == 148 && t2 == 148)
            {
                n1 = 7;
            }
            if (t1 == 199 && t2 == 199)
            {
                n1 = 8;
            }
            if (op >= 124 || op <= 125 || op == 127) n2 = 0;
            if (op == 126) n2 = 1;
            if (op == 128) n2 = 2;
            if ((op >= 109 && op <= 111) || (op >= 113 && op <= 117)) n2 = 3;
            if (op >= 106 && op <= 107) n2 = 4;
            tipo = matrizTipos[n1, n2];
            return tipo;
        }


        public string Leer_archivo()
        {
            ventana_archivos.ShowDialog();
            string url = ventana_archivos.FileName;

            return url;
        }

        // metodo sitactico
        public void Sintactico()
        {
            leer = new StreamReader(Leer_archivo());
            texto = Convert.ToString(leer.ReadToEnd()) + "  $";

            apTexto = 0;
            arrayTexto = texto.ToCharArray();
            apSimbolos = 0;
            apConstantes = 0;
            apCuadruplo = 0;

            int tam = produ.producciones(0, 0);
            String num = "", mt = "";

            // MessageBox.Show(tam+" fc");

            for (int i = tam - 1; i > 0; i--)
            {
                ejecucion.Push(produ.producciones(0, i));
            }

            while (apTexto < texto.Length)
            {
                int token = Lexico();
                num += token + " ";
                int match = Match(token);
                mt += match + " ";


                //*
                if (ejecucion.Count() == 0) break;

                int tope = ejecucion.Pop();

                while (tope >= 2000)
                {
                    Accion(tope);
                    tope = ejecucion.Pop();
                }

                while (tope < 100)
                {
                    //MessageBox.Show(""+tope + " "+(match - 1001)+"");
                    int prod = predi.Mp(tope, match - 1001);

                    if (prod >= 500)
                    {
                        MessageBox.Show("ALV error " + prod);
                        break;
                    }

                    tam = produ.producciones(prod, 0);

                    for (int i = tam; i > 0; i--)
                    {
                        ejecucion.Push(produ.producciones(prod, i));
                    }

                    tope = ejecucion.Pop();

                    while (tope >= 2000)
                    {
                        Accion(tope);
                        tope = ejecucion.Pop();
                    }
                }

                if (tope != match)
                {
                    MessageBox.Show("ALV" + tope + " != " + match);
                    break;
                }
                else
                {
                    imprimirTablas();

                }//*/ 
            }
            MessageBox.Show(num + " -- " + mt);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        public void ejecucionProg()
        {
            double cte1 = 0, cte2 = 0, cte3 = 0;
            int op1 = 0, op2 = 0, pos1 = 0, pos2 = 0, pos3 = 0;
            string str1 = "", str2 = "";
            int dir1, dir2, dir3;
            int posSimb = 0;
            for (int i = 0; i < apCuadruplo; i++)
            {
                switch (cuadruplos[i, 0])
                {
                    case 106:// &&
                        dir1 = cuadruplos[i, 1];
                        dir2 = cuadruplos[i, 2];
                        dir3 = cuadruplos[i, 3];
                        for (int l = 0; l < apSimbolos; l++)
                        {
                            if ((int)simbolos[l, 0] == dir1)
                            {
                                pos1 = l;
                            }
                            if ((int)simbolos[l, 0] == dir2)
                            {
                                pos2 = l;
                            }
                            if ((int)simbolos[l, 0] == dir3)
                            {
                                pos3 = l;
                            }
                        }
                        if ((int)simbolos[pos1, 3] == 1 && (int)simbolos[pos2, 3] == 1)
                        {
                            simbolos[pos3, 3] = 1;
                        }
                        else
                        {
                            simbolos[pos3, 3] = 0;
                        }
                        break;
                    case 107:// ||
                        dir1 = cuadruplos[i, 1];
                        dir2 = cuadruplos[i, 2];
                        dir3 = cuadruplos[i, 3];
                        for (int l = 0; l < apSimbolos; l++)
                        {
                            if ((int)simbolos[l, 0] == dir1)
                            {
                                pos1 = l;
                            }
                            if ((int)simbolos[l, 0] == dir2)
                            {
                                pos2 = l;
                            }
                            if ((int)simbolos[l, 0] == dir3)
                            {
                                pos3 = l;
                            }
                        }
                        if ((int)simbolos[pos1, 3] == 0 && (int)simbolos[pos2, 3] == 0)
                        {
                            simbolos[pos3, 3] = 0;
                        }
                        else
                        {
                            simbolos[pos3, 3] = 1;
                        }
                        break;
                    case 108:// !
                        dir1 = cuadruplos[i, 1];
                        dir2 = cuadruplos[i, 3];
                        for (int l = 0; l < apSimbolos; l++)
                        {
                            if ((int)simbolos[l, 0] == dir1)
                            {
                                pos1 = l;
                            }
                            if ((int)simbolos[l, 0] == dir2)
                            {
                                pos2 = l;
                            }
                        }
                        if ((int)simbolos[pos1, 3] == 1)
                        {
                            simbolos[pos2, 3] = 0;
                        }
                        else
                        {
                            simbolos[pos2, 3] = 1;
                        }

                        break;
                    case 109:// !=
                        dir1 = cuadruplos[i, 1];
                        dir2 = cuadruplos[i, 2];
                        dir3 = cuadruplos[i, 3];
                        if (dir1 >= 4000)
                        {
                            str1 = constantes[dir1 - 4000, 1].ToString();
                        }
                        else
                        {
                            for (int k = 0; k < apSimbolos; k++)
                            {
                                if ((int)simbolos[k, 0] == dir1)
                                {
                                    str1 = simbolos[k, 3].ToString();
                                }
                            }
                        }
                        if (dir2 >= 4000)
                        {
                            str2 = constantes[dir2 - 4000, 1].ToString();// AQUI
                        }
                        else
                        {
                            for (int j = 0; j < apSimbolos; j++)
                            {
                                if ((int)simbolos[j, 0] == dir2)
                                {
                                    str2 = simbolos[j, 3].ToString();
                                }
                            }
                        }
                        for (int k = 0; k < apSimbolos; k++)
                        {
                            if ((int)simbolos[k, 0] == dir3)
                            {
                                posSimb = k;
                            }
                        }
                        if (str1 != str2)
                        {
                            simbolos[posSimb, 3] = 1;
                        }
                        else
                        {
                            simbolos[posSimb, 3] = 0;
                        }

                        break;
                    case 110:// ==
                        dir1 = cuadruplos[i, 1];
                        dir2 = cuadruplos[i, 2];
                        dir3 = cuadruplos[i, 3];
                        if (dir1 >= 4000)
                        {
                            str1 = constantes[dir1 - 4000, 1].ToString();
                        }
                        else
                        {
                            for (int k = 0; k < apSimbolos; k++)
                            {
                                if ((int)simbolos[k, 0] == dir1)
                                {
                                    str1 = simbolos[k, 3].ToString();
                                }
                            }
                        }
                        if (dir2 >= 4000)
                        {
                            str2 = constantes[dir2 - 4000, 1].ToString();//AQUI
                        }
                        else
                        {
                            for (int j = 0; j < apSimbolos; j++)
                            {
                                if ((int)simbolos[j, 0] == dir2)
                                {
                                    str2 = simbolos[j, 3].ToString();
                                }
                            }
                        }
                        for (int k = 0; k < apSimbolos; k++)
                        {
                            if ((int)simbolos[k, 0] == dir3)
                            {
                                posSimb = k;
                            }
                        }
                        if (str1 == str2)
                        {
                            simbolos[posSimb, 3] = 1;
                        }
                        else
                        {
                            simbolos[posSimb, 3] = 0;
                        }

                        break;
                    case 111:// =
                        dir1 = cuadruplos[i, 1];
                        dir3 = cuadruplos[i, 3];
                        dir2 = 0;
                        if (dir1 >= 4000)
                        {
                            str1 = constantes[dir1 - 4000, 1].ToString();
                        }
                        else
                        {
                            for (int k = 0; k < apSimbolos; k++)
                            {
                                if ((int)simbolos[k, 0] == dir1)
                                {
                                    str1 = simbolos[k, 3].ToString();
                                }
                            }
                        }
                        for (int k = 0; k < apSimbolos; k++)
                        {
                            if ((int)simbolos[k, 0] == dir3)
                            {
                                dir2 = k;
                            }
                        }
                        simbolos[dir2, 3] = str1;

                        break;
                    case 113:// >
                        dir1 = cuadruplos[i, 1];
                        dir2 = cuadruplos[i, 2];
                        dir3 = cuadruplos[i, 3];
                        if (dir1 >= 4000)
                        {
                            cte1 = (double)constantes[dir1 - 4000, 1];
                        }
                        else
                        {
                            for (int k = 0; k < apSimbolos; k++)
                            {
                                if ((int)simbolos[k, 0] == dir1)
                                {
                                    cte1 = Convert.ToDouble(simbolos[k, 3]);
                                }
                            }
                        }
                        if (dir2 >= 4000)
                        {
                            cte2 = (double)constantes[dir2 - 4000, 1];
                        }
                        else
                        {
                            for (int j = 0; j < apSimbolos; j++)
                            {
                                if ((int)simbolos[j, 0] == dir2)
                                {
                                    cte2 = Convert.ToDouble(simbolos[j, 3]);
                                }
                            }
                        }
                        for (int k = 0; k < apSimbolos; k++)
                        {
                            if ((int)simbolos[k, 0] == dir3)
                            {
                                posSimb = k;
                            }
                        }
                        if (cte1 > cte2)
                        {
                            simbolos[posSimb, 3] = 1;
                        }
                        else
                        {
                            simbolos[posSimb, 3] = 0;
                        }

                        break;
                    case 115:// >=
                        dir1 = cuadruplos[i, 1];
                        dir2 = cuadruplos[i, 2];
                        dir3 = cuadruplos[i, 3];
                        if (dir1 >= 4000)
                        {
                            cte1 = (double)constantes[dir1 - 4000, 1];
                        }
                        else
                        {
                            for (int k = 0; k < apSimbolos; k++)
                            {
                                if ((int)simbolos[k, 0] == dir1)
                                {
                                    cte1 = Convert.ToDouble(simbolos[k, 3]);
                                }
                            }
                        }
                        if (dir2 >= 4000)
                        {
                            cte2 = (double)constantes[dir2 - 4000, 1];
                        }
                        else
                        {
                            for (int j = 0; j < apSimbolos; j++)
                            {
                                if ((int)simbolos[j, 0] == dir2)
                                {
                                    cte2 = Convert.ToDouble(simbolos[j, 3]);
                                }
                            }
                        }
                        for (int k = 0; k < apSimbolos; k++)
                        {
                            if ((int)simbolos[k, 0] == dir3)
                            {
                                posSimb = k;
                            }
                        }
                        if (cte1 >= cte2)
                        {
                            simbolos[posSimb, 3] = 1;
                        }
                        else
                        {
                            simbolos[posSimb, 3] = 0;
                        }

                        break;
                    case 116:// <
                        dir1 = cuadruplos[i, 1];
                        dir2 = cuadruplos[i, 2];
                        dir3 = cuadruplos[i, 3];
                        if (dir1 >= 4000)
                        {
                            cte1 = (double)constantes[dir1 - 4000, 1];
                        }
                        else
                        {
                            for (int k = 0; k < apSimbolos; k++)
                            {
                                if ((int)simbolos[k, 0] == dir1)
                                {
                                    cte1 = Convert.ToDouble(simbolos[k, 3]);
                                }
                            }
                        }
                        if (dir2 >= 4000)
                        {
                            cte2 = (double)constantes[dir2 - 4000, 1];
                        }
                        else
                        {
                            for (int j = 0; j < apSimbolos; j++)
                            {
                                if ((int)simbolos[j, 0] == dir2)
                                {
                                    cte2 = Convert.ToDouble(simbolos[j, 3]);
                                }
                            }
                        }
                        for (int k = 0; k < apSimbolos; k++)
                        {
                            if ((int)simbolos[k, 0] == dir3)
                            {
                                posSimb = k;
                            }
                        }
                        if (cte1 < cte2)
                        {
                            simbolos[posSimb, 3] = 1;
                        }
                        else
                        {
                            simbolos[posSimb, 3] = 0;
                        }

                        break;
                    case 117:// <=
                        dir1 = cuadruplos[i, 1];
                        dir2 = cuadruplos[i, 2];
                        dir3 = cuadruplos[i, 3];
                        if (dir1 >= 4000)
                        {
                            cte1 = (double)constantes[dir1 - 4000, 1];
                        }
                        else
                        {
                            for (int k = 0; k < apSimbolos; k++)
                            {
                                if ((int)simbolos[k, 0] == dir1)
                                {
                                    cte1 = Convert.ToDouble(simbolos[k, 3]);
                                }
                            }
                        }
                        if (dir2 >= 4000)
                        {
                            cte2 = (double)constantes[dir2 - 4000, 1];
                        }
                        else
                        {
                            for (int j = 0; j < apSimbolos; j++)
                            {
                                if ((int)simbolos[j, 0] == dir2)
                                {
                                    cte2 = Convert.ToDouble(simbolos[j, 3]);
                                }
                            }
                        }
                        for (int k = 0; k < apSimbolos; k++)
                        {
                            if ((int)simbolos[k, 0] == dir3)
                            {
                                posSimb = k;
                            }
                        }
                        if (cte1 <= cte2)
                        {
                            simbolos[posSimb, 3] = 1;
                        }
                        else
                        {
                            simbolos[posSimb, 3] = 0;
                        }

                        break;
                    case 124:// +
                        dir1 = cuadruplos[i, 1];
                        dir2 = cuadruplos[i, 2];
                        dir3 = cuadruplos[i, 3];
                        if (dir1 >= 4000)
                        {
                            cte1 = Convert.ToDouble(constantes[dir1 - 4000, 1]);
                        }
                        else
                        {
                            for (int k = 0; k < apSimbolos; k++)
                            {
                                if ((int)simbolos[k, 0] == dir1)
                                {
                                    cte1 = Convert.ToDouble(simbolos[k, 3]);
                                }
                            }
                        }
                        if (dir2 >= 4000)
                        {
                            cte2 = Convert.ToDouble(constantes[dir2 - 4000, 1]);
                        }
                        else
                        {
                            for (int j = 0; j < apSimbolos; j++)
                            {
                                if ((int)simbolos[j, 0] == dir2)
                                {
                                    cte2 = Convert.ToDouble(simbolos[j, 3]);
                                }
                            }
                        }
                        for (int k = 0; k < apSimbolos; k++)
                        {
                            if ((int)simbolos[k, 0] == dir3)
                            {
                                posSimb = k;
                            }
                        }
                        cte3 = cte1 + cte2;
                        simbolos[posSimb, 3] = cte3;

                        break;
                    case 125:// -
                        dir1 = cuadruplos[i, 1];
                        dir2 = cuadruplos[i, 2];
                        dir3 = cuadruplos[i, 3];
                        if (dir1 >= 4000)
                        {
                            cte1 = (double)constantes[dir1 - 4000, 1];
                        }
                        else
                        {
                            for (int k = 0; k < apSimbolos; k++)
                            {
                                if ((int)simbolos[k, 0] == dir1)
                                {
                                    cte1 = Convert.ToDouble(simbolos[k, 3]);
                                }
                            }
                        }
                        if (dir2 >= 4000)
                        {
                            cte2 = (double)constantes[dir2 - 4000, 1];
                        }
                        else
                        {
                            for (int j = 0; j < apSimbolos; j++)
                            {
                                if ((int)simbolos[j, 0] == dir2)
                                {
                                    cte2 = Convert.ToDouble(simbolos[j, 3]);
                                }
                            }
                        }
                        for (int k = 0; k < apSimbolos; k++)
                        {
                            if ((int)simbolos[k, 0] == dir3)
                            {
                                posSimb = k;
                            }
                        }
                        cte3 = cte1 - cte2;
                        simbolos[posSimb, 3] = cte3;

                        break;
                    case 126:// /
                        dir1 = cuadruplos[i, 1];
                        dir2 = cuadruplos[i, 2];
                        dir3 = cuadruplos[i, 3];
                        if (dir1 >= 4000)
                        {
                            cte1 = (double)constantes[dir1 - 4000, 1];
                        }
                        else
                        {
                            for (int k = 0; k < apSimbolos; k++)
                            {
                                if ((int)simbolos[k, 0] == dir1)
                                {
                                    cte1 = Convert.ToDouble(simbolos[k, 3]);
                                }
                            }
                        }
                        if (dir2 >= 4000)
                        {
                            cte2 = (double)constantes[dir2 - 4000, 1];
                        }
                        else
                        {
                            for (int j = 0; j < apSimbolos; j++)
                            {
                                if ((int)simbolos[j, 0] == dir2)
                                {
                                    cte2 = Convert.ToDouble(simbolos[j, 3]);
                                }
                            }
                        }
                        for (int k = 0; k < apSimbolos; k++)
                        {
                            if ((int)simbolos[k, 0] == dir3)
                            {
                                posSimb = k;
                            }
                        }
                        cte3 = cte1 / cte2;
                        simbolos[posSimb, 3] = cte3;

                        break;
                    case 127:// *
                        dir1 = cuadruplos[i, 1];
                        dir2 = cuadruplos[i, 2];
                        dir3 = cuadruplos[i, 3];
                        if (dir1 >= 4000)
                        {
                            cte1 = (double)constantes[dir1 - 4000, 1];
                        }
                        else
                        {
                            for (int k = 0; k < apSimbolos; k++)
                            {
                                if ((int)simbolos[k, 0] == dir1)
                                {
                                    cte1 = Convert.ToDouble(simbolos[k, 3]);
                                }
                            }
                        }
                        if (dir2 >= 4000)
                        {
                            cte2 = (double)constantes[dir2 - 4000, 1];
                        }
                        else
                        {
                            for (int j = 0; j < apSimbolos; j++)
                            {
                                if ((int)simbolos[j, 0] == dir2)
                                {
                                    cte2 = Convert.ToDouble(simbolos[j, 3]);
                                }
                            }
                        }
                        for (int k = 0; k < apSimbolos; k++)
                        {
                            if ((int)simbolos[k, 0] == dir3)
                            {
                                posSimb = k;
                            }
                        }
                        cte3 = cte1 * cte2;
                        simbolos[posSimb, 3] = cte3;

                        break;
                    case 128:// %
                        dir1 = cuadruplos[i, 1];
                        dir2 = cuadruplos[i, 2];
                        dir3 = cuadruplos[i, 3];
                        if (dir1 >= 4000)
                        {
                            cte1 = Convert.ToDouble(constantes[dir1 - 4000, 1]);
                        }
                        else
                        {
                            for (int k = 0; k < apSimbolos; k++)
                            {
                                if ((int)simbolos[k, 0] == dir1)
                                {
                                    cte1 = Convert.ToDouble(simbolos[k, 3]);
                                }
                            }
                        }
                        if (dir2 >= 4000)
                        {
                            cte2 = Convert.ToDouble(constantes[dir2 - 4000, 1]);
                        }
                        else
                        {
                            for (int j = 0; j < apSimbolos; j++)
                            {
                                if ((int)simbolos[j, 0] == dir2)
                                {
                                    cte2 = Convert.ToDouble(simbolos[j, 3]);
                                }
                            }
                        }
                        for (int k = 0; k < apSimbolos; k++)
                        {
                            if ((int)simbolos[k, 0] == dir3)
                            {
                                posSimb = k;
                            }
                        }
                        cte3 = cte1 % cte2;
                        simbolos[posSimb, 3] = cte3;

                        break;
                    case 400://goto
                        i = cuadruplos[i, 3] - 1;
                        break;
                    case 401://gotoF
                        int sim = 0;
                        for (int j = 0; j < apSimbolos; j++)
                        {
                            if (cuadruplos[i, 1] == (int)simbolos[j, 0])
                            {
                                sim = j;
                            }
                        }
                        if ((int)simbolos[sim, 3] == 0)
                        {
                            i = cuadruplos[i, 3] - 1;
                        }
                        break;
                    case 402: //gotoV
                        int sim1 = 0;
                        for (int j = 0; j < apSimbolos; j++)
                        {
                            if (cuadruplos[i, 1] == (int)simbolos[j, 0])
                            {
                                sim1 = j;
                            }
                        }
                        if ((int)simbolos[sim1, 3] == 1)
                        {
                            i = cuadruplos[i, 3] - 1;
                        }
                        break;
                    case 144://read
                        dir1 = cuadruplos[i, 3];
                        int s = 0;
                        Object valor1 = (Object)Microsoft.VisualBasic.Interaction.InputBox("Ingresa el valor");

                        for (int k = 0; k < apSimbolos; k++)
                        {
                            if ((int)simbolos[k, 0] == dir1)
                            {
                                simbolos[k, 3] = valor1;
                                s = k;
                            }
                        }
                        int exi = -1;
                        for (int j = 0; j < apConstantes; j++)
                        {
                            if (constantes[j, 1].ToString() == valor1.ToString())
                            {
                                exi = j;
                            }
                        }
                        if (exi == -1)
                        {
                            GeConstantes(apConstantes + 4000, valor1, (int)simbolos[s, 2]);
                        }


                        break;
                    case 145://write
                        dir1 = cuadruplos[i, 3];
                        if (dir1 >= 4000)
                        {
                            str1 = constantes[dir1 - 4000, 1].ToString();
                        }
                        else
                        {
                            for (int k = 0; k < apSimbolos; k++)
                            {
                                if ((int)simbolos[k, 0] == dir1)
                                {
                                    str1 = simbolos[k, 3].ToString();
                                }
                            }
                        }

                        MessageBox.Show(str1);
                        break;
                }
                imprimirTablas();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
