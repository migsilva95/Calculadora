using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CalculadoraCompleta.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            // preparar os primeiros valores da calculadora
            ViewBag.Visor = 0;
            Session["operador"] = "";
            Session["limpaVisor"] = true;
            return View();
        }

        // POST: Home
        [HttpPost]
        public ActionResult Index(string bt, string visor)
        {
            // determinar a ação a executar
            switch (bt)
            {
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                case "0":
                    // recupera o resultado da decisão sobre a limpeza do visor
                    bool limpaEcra = (bool)Session["limpaVisor"];
                    // processa a escrita do visor
                    if (limpaEcra || visor.Equals("0")) visor = bt;
                    else visor += bt;
                    // marcar o visor para continuar a escrita do operando
                    Session["limpaVisor"] = false;
                    break;
                case "+/-":
                    visor = Convert.ToDouble(visor) * (-1) + "";
                    break;
                case ",":
                    if (!visor.Contains(",")) visor += ",";
                    break;
                case "+":
                case "-":
                case "x":
                case ":":
                case "=":
                    // se é a primeira vez que pressiono um operador
                    if (!((string)Session["operador"]).Equals(""))
                    {
                        // agora é q se vai fazer a 'conta'
                        // obter os operandos
                        double primeiroOperando = Convert.ToDouble((string)Session["primeiroOperando"]);
                        double segundoOperando = Convert.ToDouble(visor);
                        // escolher a operação a fazer com o operador anterior
                        switch ((string)Session["operador"])
                        {
                            case "+":
                                visor = primeiroOperando + segundoOperando + "";
                                break;
                            case "-":
                                visor = primeiroOperando - segundoOperando + "";
                                break;
                            case "x":
                                visor = primeiroOperando * segundoOperando + "";
                                break;
                            case ":":
                                visor = primeiroOperando / segundoOperando + "";
                                break;
                        }
                        if (bt.Equals("=")) bt = "";
                    }
                    // preservar os valores fornecidos para operações futuras
                    Session["primeiroOperando"] = visor;
                    Session["operador"] = bt;
                    // marcar o visor para 'limpeza'
                    Session["limpaVisor"] = true;
                    break;
                case "C":
                    // vamos limpar a calculadora,
                    // isto é, fazer um 'reset' total
                    visor = "0";
                    Session["operador"] = "";
                    Session["limpaVisor"] = true;
                    break;
            }

            // enviar o resultado para a View
            ViewBag.Visor = visor;
            return View();
        }
    }
}