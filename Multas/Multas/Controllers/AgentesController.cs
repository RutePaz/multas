using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Multas.Models;

namespace Multas.Controllers
{
    [Authorize(Roles = "Agentes, GestaoDePessoal")]
    //tem premissao de acesso quem for agente ou gestaodepessoaç
    public class AgentesController : Controller
    {
        //cria uma variável
        
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Agentes
        /// <summary>
        /// lista todos os agente 
        /// </summary>
        /// <returns></returns>
        
        public ActionResult Index()
        {
            //db.Agentes.ToList() -> em sql: SELECT * FROM Agentes;
            //enviar para a View uma lista com todos os agente da base de dados 

            //recuperar os dados pessoais de uma pessoa que se autenticou 
            //var dadosPessoais = db.Users.Find(User.Identity.GetUserId());
            
            var dadosPessoais = db.Utilizadores.Where(u => u.NomeRegistoDoUtilizador.Equals(User.Identity.Name)).FirstOrDefault();
            //FirstOrDfault para retornar um elemento unico, sem este comando retornaria uma lista
            //agora, com este objeto, já posso utilizar
            //os dados pessoais de um utilizador no meu programa 
            //por exemplo
            Session["nomeUtilizador"] = dadosPessoais.NomeProprio + "" + dadosPessoais.Apelido;


            //obter a lista de todos os agente 
            //em SQL: Select * from Agentes order by nome
            var listaDeAgentes =db.Agentes.OrderBy(a=>a.Nome).ToList();

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
            //fazer a mesma proteção que foi feita no EDIT (GET)
            //entrega á View os dados do Agente encontrado 
            return View(agente);
        }

        // GET: Agentes/Create
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = " GestaoDePessoal")]
        //Neste método, apenas o GestaoDePessoal pode criar 
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
        [Authorize(Roles = " GestaoDePessoal")]
        public ActionResult Create([Bind(Include = "Nome,Esquadra")] Agentes agente, HttpPostedFileBase uploadFotografia) {
            //agente - parametro de entrada
            //include -  dados que vêm da view

            //escrever os dados de um novo Agente na BD
            //especificar o ID do novo Agente
            //testar s ehá resgistos na tabela dos Agentes (count - devolve o número de registos) 
            //if (db.Agentes.Count() != 0) {}
            //ou entã, usar a instrução Try/Catch
            int idNovoAgente = 0;
            try
            {
                idNovoAgente = db.Agentes.Max(a => a.ID) + 1;
            }
            catch (Exception) {
                idNovoAgente = 1;
            }
               
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
                try
                {
                    //adiciona o novo agente á BD
                    db.Agentes.Add(agente);
                    //faz 'Commit' às alterações 
                    db.SaveChanges();
                    //redireciona para a pagina de Index
                    //escrever o ficheiro 
             
                    //se tudo coorer bem, redireciona para a pagina de index
                    return RedirectToAction("Index");
                }
                catch (Exception ex) {
                    //ModelState-objecto que identifica o modelo 
                    ModelState.AddModelError("", "Houve um erro com a criação do novo agente... ");

                    ///se existir uma classe chamada 'Erros.cs'
                    ///iremos nela registar os dados do erro 
                    ///-cirar um objeto desta classe
                    ///-atribuir a esse objeto os dados do erro
                    ///     -nome do controller
                    ///     -nome do método
                    ///     -data + hora do erro 
                    ///     - mensagem do erro (ex)
                    ///     -dados que se tentavam inserir 
                    ///     -outros dados considerados relevantes 
                    /// -guardar o objeto na BD
                    /// -notificar um gestor do sistema, por email, ou por outro meio do erro e dos seus dados
                    
                }
            }
            //se houver um erro, representa os dados do Agente na View
            return View(agente);
        }

        // GET: Agentes/Edit/5
        /// <summary>
        /// apresentar na view os dados de um agente para 
        /// eventual edição
        /// </summary>
        /// <param name="id">identifica o agente a editar </param>
        /// <returns>view</returns>
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
            //existe agente 
            //contudo, será que posso editá-lo
            if (User.IsInRole("GestaoDePessoal") || User.Identity.Name.Equals(agente.UserName))
            {
                //entrega á View os dados do Agente encontrado 
                return View(agente);
            }
            else
            {
                //não ha permissão para editar o Agente 
                return RedirectToAction("Index");

            }
        }

        // POST: Agentes/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// concretiza a edição dos dados de um agente 
        /// </summary>
        /// <param name="agentes">dados do agente a alterar</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nome,Esquadra,Fotografia, UserName")] Agentes agentes)
        {
            ///se o utilizador pertence á role 'GestaoDePessoa', pode efetuar a edição sem qualquer restrição 
            ///se o utilizador não pertence á role acima referida  e não é o dono, nada se pode fazer 
            ///se o utilizador nãp pertence á role e é o dono dos dados, apenas pode alterar o 'nome' e a 'fotografia'
            ///tarefas:
            ///     1- pesquisar os dados antigos do agente na BD
            ///     2-substituir nos dados novos, o valor da 'esquadra' pelos dados antigos da 'esquadra'
            ///     3-guardar dados na BD 
            ///     nota: claro que a validação do Nome e da Fotografia também tem de acontecer 
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
        [Authorize(Roles = " GestaoDePessoal")]
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
        [Authorize(Roles = " GestaoDePessoal")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Agentes agente = db.Agentes.Find(id);
                //remove o Agente da DB
                db.Agentes.Remove(agente);
                //Commit
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex) {
                ModelState.AddModelError("", string.Format("Não é possível apagar o Agente nº {0}-{1}, porque há multas associadas a ele", id, agente.Nome)
                );
            //se cheguei aqui é porque houve um problema 
            //devolvo os dados do agente á view
            return View(agente);
            }

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
//int (sem ?) porque é pouco provavel que este valor sera alterado