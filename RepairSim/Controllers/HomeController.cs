using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RepairSim.Models.ViewModel;
using RepairSim.Models.BlocoPrincipal;
using RepairSim.Models.Entidades;

namespace RepairSim.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ValoresEntradaViewModel model = new ValoresEntradaViewModel();
            //Fixo para 
           

            return View(model);
        }
        
        [HttpPost]
         public IActionResult Index(ValoresEntradaViewModel model)
        {
            AdicionarFuncionarios(model);
            ProgramaPrincipal sim = new ProgramaPrincipal();
            sim.Inicializador(model);
            return View("Report",sim);
        }

        private void AdicionarFuncionarios(ValoresEntradaViewModel model)
        {
            model.Tecnicos.Add(new Tecnico() { Experiencia = 1 });
            model.Tecnicos.Add(new Tecnico() { Experiencia = 1 });

            model.ColaboradoresArmazem.Add(new ColaboradorArmazem() { Experiencia = 1 });
            model.ColaboradoresArmazem.Add(new ColaboradorArmazem() { Experiencia = 1 });
            
            model.ColaboradoresLogistica.Add(new ColaboradorLogistica() { Experiencia = 1 });
            model.ColaboradoresLogistica.Add(new ColaboradorLogistica() { Experiencia = 1 });
            model.Testers.Add(new Tester() { Experiencia = 1 });
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        /// <summary>
        /// Runs the simulation with the variables passed in the model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult AddLineTecnico()
        {
            var model = new Tecnico();
            return PartialView("~/Views/Shared/EditorTemplate/_Tecnico.cshtml", model);
        }
        [HttpPost]
        public ActionResult AddLineArmazem()
        {
            var model = new ColaboradorArmazem();
            var a = PartialView("~/Views/Shared/EditorTemplate/_Armazem.cshtml",model);
            return a;
        }
        public ActionResult AddLineLogistica()
        {
            var model = new ColaboradorLogistica();
            return PartialView("~/Views/Shared/EditorTemplate/_Logistica.cshtml",model);
        }
        public ActionResult AddLineTester()
        {
            var model = new Tester();
            return PartialView("~/Views/Shared/EditorTemplate/_Tester.cshtml",model);
        }
    }
}
