using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
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
        /// <summary>
        /// lista todos os agentes 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //db.Agentes.ToList() -> em sql: SELECT * FROM Agentes;
            //enviar para a View uma lista com todos os agente da base de dados 

            //obter a lista de todos os agente 
            //em SQL: Select * from Agentes order by nome
            var listaDeAgentes=db.Agentes.ToList().OrderBy(a=>a.Nome);

            return View(listaDeAgentes);
        }

        // GET: Agentes/Details/5
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int? id)
        {
            //int? preenchimento facultativo (em sql contrário de NOT NULL)
            //proteção para caso de não ter sido fornecido um ID valido
            if (id == null)
            {   //instrução original
                //devolve um erro quando não há id, logo não é possível pesquisar por um agente 
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                //redirecionar para uma página que seja controlável
                return RedirectToAction("Index");
            }
            //procura na BD, o Agente cujo ID for fornecido
            Agentes agente = db.Agentes.Find(id);
            //proteção para caso não se ter encontrado o Aagente que tenha o ID fornecido
            if (agente == null)
            {
                //o agente não foi encontrado logo, gera-se uma mensagem de erro
                //return HttpNotFound();

                //redirecionar para uma página que seja controlável
                return RedirectToAction("Index");

            }
            //entrega á View os dados do Agente encontrado 
            return View(agente);
        }

        // GET: Agentes/Create
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
            //apresneta a View para se inserir um novo agente
        {
            return View();
        }

        // POST: Agentes/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="agente"></param>
        /// <param name="uploadFotografia"></param>
        /// <returns></returns>
        //anotador para HTTP POST
        [HttpPost]
        //anortador para proteção por roube de identidade
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nome,Esquadra")] Agentes agente, HttpPostedFileBase uploadFotografia) {
            //agente - parametro de entrada
            //include -  dados que vêm da view
            
            //escrever os dados de um novo Agente na BD
            //especificar o ID do novo Agente
            int idNovoAgente = db.Agentes.Max(a=>a.ID)+1;
            //guardar o ID do novo Agente
            agente.ID = idNovoAgente;
            //Especificar (escolher) o nome do ficheiro
             string nomeImagem = "Agente_"+idNovoAgente + ".jpg";
            //var auxiliar
            string path = "";
            //validar se a imagem foi fornecida
            if (uploadFotografia != null)
            {
                //o ficheiro foi fornecido 
                //Validar se o que foi fornecido é uma imagem 
                //formatar o tamanho da imagem

                //criar o caminho completo até ao sítio onde o ficheiro será guardado
                path = Path.Combine(Server.MapPath("~imagens/"), nomeImagem + ".jpg");

                //guardar o nome do ficheiro 
                agente.Fotografia = nomeImagem;


            }
            else {
                //não foi fornecido qualquer ficheiro 
                //gerar uma mensagem de erro
                ModelState.AddModelError("", "Não foi fornecida nenhuma imagem...");
                //devolver o controlo á View 
                return View(agente);

            }
            //escrever o ficheiro com a fotografia no disco rígido, na pasta 'imagens'
            //guardar o nome escolhido na BD

            //ModelState.IsValid -> 
            //adiciona o novoconfronta os dados fornecidos da View com as exigências do Modelo              
            if (ModelState.IsValid)
            { 
                //adiciona o novo agente á BD
                db.Agentes.Add(agente);
                //faz 'Commit' às alterações 
                db.SaveChanges();
                //redireciona para a pagina de Index
                //escrever o ficheiro 
                uploadFotografia.SaveAs(path);
                return RedirectToAction("Index");
            }
            //se houver um erro, representa os dados do Agente na View
            return View(agente);
        }

        // GET: Agentes/Edit/5
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int? id)
        {
            //int? preenchimento facultativo (em sql contrário de NOT NULL)
            //proteção para caso de não ter sido fornecido um ID valido
            if (id == null)
            {   //instrução original
                //devolve um erro quando não há id, logo não é possível pesquisar por um agente 
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                //redirecionar para uma página que seja controlável
                return RedirectToAction("Index");
            }
            //procura na BD, o Agente cujo ID for fornecido
            Agentes agente = db.Agentes.Find(id);
            //proteção para caso não se ter encontrado o Aagente que tenha o ID fornecido
            if (agente == null)
            {
                //o agente não foi encontrado logo, gera-se uma mensagem de erro
                //return HttpNotFound();

                //redirecionar para uma página que seja controlável
                return RedirectToAction("Index");

            }
            //entrega á View os dados do Agente encontrado 
            return View(agente);
        }

        // POST: Agentes/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="agentes"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nome,Esquadra,Fotografia")] Agentes agentes)
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
        /// <summary>
        /// Apresenta na view os dados de um Aagente, com vista á sua ,eventual, eliminação
        /// </summary>
        /// <param name="id">Identificador do Agente</param>
        /// <returns></returns>
        public ActionResult Delete(int? id)
        {
            //verificar se foi fornecido um ID válido 
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            //pesquisar pelo Agente, cujo ID foi fornecido 
            Agentes agente = db.Agentes.Find(id);
            //verificar se o Agente foi verificado 
            if (agente == null)
            {
                //o Agente não existe
                //redireciona para a página inicial
                return RedirectToAction("Index");
            }
            return View(agente);
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
