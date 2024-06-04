using System.Data.Odbc;

class Clinica
{
    // Connection string for the ODBC data source
    static string stringDeConexao = "DSN=ClinicaMySQLODBC;UID=root;PWD=;";
    static string nomeTabela = "paciente";

    static void consulta()
    {
        using (OdbcConnection connection = new OdbcConnection(stringDeConexao))
        {
            string query = $"SELECT * FROM {nomeTabela}";
            OdbcCommand comandoOdbc = new OdbcCommand(query, connection);
            try
            {
                connection.Open();
                OdbcDataReader reader = comandoOdbc.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine($"ID:  {reader["id"]}       NOME:  {reader["nome"]}  IDADE: {reader["idade"]}     MORADA: {reader["morada"]}        ENTRADA: {reader["data_entrada"]}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao selecionar dados: " + ex.Message);
            }
        }
    }
    static void consultaPorId(int id)
    {
        Paciente paciente = obterPaciente(id);
        Console.WriteLine($"ID:  {paciente.Id}       NOME:  {paciente.Nome}  IDADE: {paciente.Idade}     MORADA: {paciente.Morada}        ENTRADA: {paciente.DataEntrada}");
    }
    static void actualizar(Paciente paciente)
    {
        using (OdbcConnection connection = new OdbcConnection(stringDeConexao))
        {
            string query = $"UPDATE {nomeTabela} SET nome = ?, morada = ?, idade=?, diagnostico = ?  WHERE id = ?";
            OdbcCommand comandoOdbc = new OdbcCommand(query, connection);
            comandoOdbc.Parameters.AddWithValue("@nome", paciente.Nome);
            comandoOdbc.Parameters.AddWithValue("@morada", paciente.Morada);
            comandoOdbc.Parameters.AddWithValue("@idade", paciente.Idade);
            comandoOdbc.Parameters.AddWithValue("@diagnostico", paciente.Diagnostico);
            comandoOdbc.Parameters.AddWithValue("@id", paciente.Id);

            try
            {
                connection.Open();
                int rowsAffected = comandoOdbc.ExecuteNonQuery();
                Console.WriteLine($"Actualizado com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao atualizar dados: " + ex.Message);
            }
        }
    }
    static void inserir(Paciente paciente)
    {

        using (OdbcConnection connection = new OdbcConnection(stringDeConexao))
        {
            string query = $"INSERT INTO {nomeTabela} (nome, idade, morada, diagnostico, data_entrada) VALUES ( ?, ?, ?, ?, ?)";
            OdbcCommand comandoOdbc = new OdbcCommand(query, connection);
            comandoOdbc.Parameters.AddWithValue("@nome", paciente.Nome);
            comandoOdbc.Parameters.AddWithValue("@idade", paciente.Idade);
            comandoOdbc.Parameters.AddWithValue("@morada", paciente.Morada);
            comandoOdbc.Parameters.AddWithValue("@diagnostico", paciente.Diagnostico);
            comandoOdbc.Parameters.AddWithValue("@data_entrada", paciente.DataEntrada);

            try
            {
                connection.Open();
                int rowsAffected = comandoOdbc.ExecuteNonQuery();
                Console.WriteLine($"Linhas Inseridas: {rowsAffected}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao inserir dados: " + ex.Message);
            }
        }

    }
    static void eliminar(int id)
    {
        using (OdbcConnection connection = new OdbcConnection(stringDeConexao))
        {
            string query = $"DELETE FROM {nomeTabela} WHERE id = ?";
            OdbcCommand comandoOdbc = new OdbcCommand(query, connection);
            comandoOdbc.Parameters.AddWithValue("@id", id);

            try
            {
                connection.Open();
                int rowsAffected = comandoOdbc.ExecuteNonQuery();
                Console.WriteLine($"Eliminado com sucesso! {rowsAffected}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao excluir dados: " + ex.Message);
            }
        }
    }
    static Paciente obterPaciente(int id)
    {
        Paciente paciente = new Paciente();
         using (OdbcConnection connection = new OdbcConnection(stringDeConexao))
        {
            string query = $"SELECT * FROM {nomeTabela} WHERE id = ?";
            OdbcCommand comandoOdbc = new OdbcCommand(query, connection);
            comandoOdbc.Parameters.AddWithValue("@ID", id);
            try
            {
                connection.Open();
                OdbcDataReader reader = comandoOdbc.ExecuteReader();

                while (reader.Read())
                {
                    paciente.Id = int.Parse(reader["id"].ToString());
                    paciente.Idade = int.Parse(reader["idade"].ToString());
                    paciente.Nome = (string?)reader["nome"];
                    paciente.Morada = (string?)reader["morada"];
                    paciente.Diagnostico = (string?)reader["diagnostico"];
                    DateTime date = (DateTime)reader["data_entrada"];
                    paciente.DataEntrada = date.ToString("dd-MM-yyyy HH:mm:ss");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao selecionar dados: " + ex.Message);
            }
        }
        return paciente;
    }
    // MENUS
    static int menu()
    {
        Console.WriteLine();
        Console.Out.Flush();
        int opcao;
        Console.WriteLine("BEM-VINDO AO PROGRAMA CLINICA == USANDO ODBC");
        Console.WriteLine("1 - Listar registos");
        Console.WriteLine("2 - Inserir registo");
        Console.WriteLine("3 - Listar por ID");
        Console.WriteLine("4 - Actualizar");
        Console.WriteLine("5 - Eliminar");
        Console.WriteLine("6 - Sair");
        opcao = lerIntDoConsole("Selecione a opção desejada: ");
        Console.WriteLine();
        Console.Out.Flush();
        return opcao;
    }
    static void menuEliminar()
    {
        int id;
        id = lerIntDoConsole("Insira o ID: ");
        if (existe(id))
        {
            eliminar(id);
        }
        else
        {
            Console.WriteLine("O Id Inserido não foi encontrado!");
        }
    }
    static void menuActualizar()
    {
        int id;
        id = lerIntDoConsole("Insira o ID: ");
        if (existe(id))
        {
            Paciente paciente =   obterPaciente(id);
            Console.WriteLine("DADOS ACTUAIS");
            Console.WriteLine($"ID:  {paciente.Id}       NOME:  {paciente.Nome}  IDADE: {paciente.Idade}     MORADA: {paciente.Morada}        ENTRADA: {paciente.DataEntrada}");
            paciente = editarPaciente(paciente);
            actualizar(paciente);
        }
        else
        {
            Console.WriteLine("O Id Inserido não foi encontrado!");
        }
    }
    static void menuInserir()
    {
        Paciente paciente = criarPaciente();
        inserir(paciente);
    }
    static Paciente criarPaciente()
    {
        Paciente paciente = new Paciente();
        Console.WriteLine("Inserir dados para novo paciente");
        Console.Write("Nome: ");
        paciente.Nome = Console.ReadLine();

        paciente.Idade = lerIntDoConsole("Idade: ");

        Console.Write("Morada");
        paciente.Morada = Console.ReadLine();

        Console.Out.Flush();
        Console.Write("Diagnostico: ");
        paciente.Diagnostico = Console.ReadLine();

        Console.Out.Flush();
        Console.Write("Data Entrada: ");
        paciente.DataEntrada = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        Console.Out.Flush();


        return paciente;
    }
    static Paciente editarPaciente(Paciente paciente)
    {

        Console.WriteLine("Inserir dados de actualização paciente (0 - para pular)");
        Console.Write("Nome: ");
        string nome = Console.ReadLine();
        
        if(!isInt(nome))
        {
            paciente.Nome = nome;
        }
        Console.Out.Flush();
        int idade = lerIntDoConsole("Idade");
        if(isInt(idade.ToString()) && idade>0)
        {
            paciente.Idade = idade;
        }

        Console.Write("Morada: ");
        string morada = Console.ReadLine();
        if(!isInt(morada))
        {
            paciente.Morada = morada;
        }
        Console.Out.Flush();

        Console.Write("Diagnóstico: ");
        string diagnostico = Console.ReadLine();
        if(!isInt(diagnostico))
        {
            paciente.Diagnostico = diagnostico;
        }
        Console.Out.Flush();

        return paciente;
    }
    static void menuObterPorId()
    {
        int id;
        id = lerIntDoConsole("Insira o ID: ");
        if (existe(id))
        {
            consultaPorId(id);
        }
        else
        {
            Console.WriteLine("O Id Inserido não foi encontrado!");
        }
    }
    static bool existe(int id)
    {
        bool existe = false;
        using (OdbcConnection connection = new OdbcConnection(stringDeConexao))
        {
            string query = $"SELECT * FROM {nomeTabela} WHERE id = ?";
            OdbcCommand comandoOdbc = new OdbcCommand(query, connection);
            comandoOdbc.Parameters.AddWithValue("@ID", id);
            try
            {
                connection.Open();
                Console.WriteLine(comandoOdbc.ExecuteNonQuery());
                if (comandoOdbc.ExecuteNonQuery() >= 1)
                {
                    existe = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao selecionar dados: " + ex.Message);
            }
        }
        return existe;
    }
    static int lerIntDoConsole(string texto)
    {
        int numero = 0;
        bool entradaValida = false;
        while (!entradaValida)
        {
            Console.Write(texto);
            String entrada = Console.ReadLine();
            if (int.TryParse(entrada, out numero))
            {
                entradaValida = true;
            }
            else
            {
                Console.WriteLine("Entrada Inválida! Insira um número");
            }
        }
        Console.Out.Flush();
        return numero;
    }
    static bool isInt(string numero)
    {
        int numberTest;
        if(int.TryParse(numero,out numberTest))
        {
            return true;
        }else{
            return false;
        }
    }
    static void Main()
    {
        bool controlador = true;

        while (controlador)
        {
            int opcao = menu();
            switch (opcao)
            {
                case 1:
                    consulta();
                    break;

                case 2:
                    menuInserir();
                    break;
                case 3:
                    menuObterPorId();
                    break;
                case 4:
                    menuActualizar();
                    break;
                case 5:
                    menuEliminar();
                    break;
                case 6:
                    controlador = false;
                    break;
                default:
                    Console.WriteLine("Entrada inválida!");
                    Console.WriteLine("Pressione qualquer tecla para voltar...");
                    Console.ReadLine();

                    break;
            }
        }
        Console.WriteLine("Pressione qualquer tecla para sair...");
        Console.ReadLine();

    }
}