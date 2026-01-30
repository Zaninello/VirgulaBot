using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;

var bot = new TelegramBotClient("7663425286:AAFUyFaORZvX_huW2WYKQHB3h5QDGCbZ1Tk");
bot.StartReceiving(Update, Error);
Console.ReadLine();

static void Update(ITelegramBotClient bot, Update update, CancellationToken ct)
{
    var conteudo = update.Message.Text;
    var conexao = new Banco();
    if (conteudo == "/list")
    {
        conexao.Mensagens
            .ToList()
            .ForEach(
                msg => bot.SendMessage(
                    update.Message.Chat.Id, 
                    $"mensagem: {msg.Conteudo} | horario: {msg.Data}"
                )
            );
        
        return;
    }


    var nome = update.Message.Chat.FirstName;
    var mensagem = new Mensagem() 
    {
        Conteudo = conteudo,
        Data = DateTime.Today,
        Nome = nome
    };
    conexao.Add(mensagem);
    conexao.SaveChanges();
}
static void Error(ITelegramBotClient bot, Exception exception, CancellationToken ct)
{ 
    
}
public class Mensagem
{ 
    public int Id { get; set; }
    public string Conteudo { get; set; }
    public DateTime Data { get; set; }
    public string Nome { get; set; }
}

public class Banco : DbContext
{ 
    public DbSet<Mensagem> Mensagens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("Banco");
    }
}