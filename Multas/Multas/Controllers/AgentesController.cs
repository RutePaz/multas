using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Multas.Models;

namespace Multas.Controllers
{
    public class AgentesController : Controller
    {
        //cria uma variável 
        private MultasDB db = new MultasDB();

        // GET: Agentes
        public ActionResult Index()
        {
            //db.Agentes.ToList() -> em sql: SELECT * FROM Agentes;
            //enviar para a View uma lista com todos os agentes da base de dados 
            return View(db.Agentes.ToList());
        }

        // GET: Agentes/Details/5
        public ActionResult Details(int? id)
        {
            //int? preenchimento facultativo (em sql contrário de NOT NULL)
            //proteção para caso de não ter sido fornecido um ID valido
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //procura na BD, o Agente cujo ID for fornecido
            Agentes agentes = db.Agentes.Find(id);
            //proteção para caso não se ter encontrado o Aagente que tenha o ID fornecido
            if (agentes == null)
            {
                return HttpNotFound();
            }
            //entrega á View os dados do Agente encontrado 
            return View(agentes);
        }

        // GET: Agentes/Create
        public ActionResult Create()
            //apresneta a View para se inserir um novo agente
        {
            return View();
        }

        // POST: Agentes/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        //anotador para HTTP POST
        [HttpPost]
        //anortador para proteção por roube de identidade
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,NOME,Esquadra,Fotografia")] Agentes agentes)
            //agentes - parametro de entrada
            //include -  dados que vêm da view
        {
            //escrever os dados de um novo Agente na BD

            //ModelState.IsValid -> confronta os dados fornecidos da View com as exigências do Modelo              
            if (ModelState.IsValid)
            {
            //adiciona o novo agente á BD 
                db.Agentes.Add(agentes);
                //faz 'Commit' às alterações 
                db.SaveChanges();
                //redireciona para a pagina de Indes
                return RedirectToAction("Index");
            }
            //se houver um erro, representa os dados do Agente na View
            return View(agentes);
        }

        // GET: Agentes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agentes agentes = db.Agentes.Find(id);
            if (agentes == null)
            {
                return HttpNotFound();
            }
            return View(agentes);
        }

        // POST: Agentes/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,NOME,Esquadra,Fotografia")] Agentes agentes)
        {
            if (ModelState.IsValid)
            {
                //neste caso já existe um Agente
                //apenas edita os dados 
                db.Entry(agentes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(agentes);
        }

        // GET: Agentes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agentes agentes = db.Agentes.Find(id);
            if (agentes == null)
            {
                return HttpNotFound();
            }
            return View(agentes);
        }

        // POST: Agentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Agentes agentes = db.Agentes.Find(id);
            //remove o Agente da DB
            db.Agentes.Remove(agentes);
            //Commit
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
