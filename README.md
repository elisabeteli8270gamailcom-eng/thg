# 🚀 FiveM Optimizer

Um aplicativo Windows para otimizar seu sistema e melhorar o desempenho do FiveM (GTA V Multiplayer).

## ✨ Funcionalidades

- 🧹 **Limpeza de Arquivos Temporários** - Remove arquivos desnecessários do sistema
- 💾 **Limpeza de Cache** - Limpa a lixeira e cache do sistema
- ⚡ **Otimização de Memória** - Libera memória não utilizada
- 🔧 **Informações do Sistema** - Mostra detalhes da configuração do seu PC
- 📊 **Monitor de Recursos** - Acompanha uso de CPU, memória e disco

## 🛠️ Requisitos

- **Windows 10/11** ou superior
- **.NET 8.0 Runtime** ou superior
- **Privilégios de Administrador**
- **Visual Studio 2022** (para compilar do código-fonte)

## 📦 Instalação

### Opção 1: Compilar do Código-Fonte

1. Clone o repositório:
```bash
git clone https://github.com/elisabeteli8270gamailcom-eng/thg.git
cd thg
```

2. Abra a solução no Visual Studio:
```bash
FiveMOptimizer.sln
```

3. Compile o projeto (Release):
   - Build > Build Solution
   - Ou pressione `Ctrl + Shift + B`

4. Execute o programa em `bin/Release/FiveMOptimizer.UI.exe`

### Opção 2: Executável Pronto (Se Disponível)

Baixe o `.exe` pronto na seção **Releases** do GitHub.

## 🚀 Como Usar

1. Execute o aplicativo como **Administrador**
2. Selecione as otimizações que deseja executar:
   - ✅ Limpar Arquivos Temporários
   - ✅ Limpar Cache
   - ✅ Otimizar Memória
   - ✅ Outras opções disponíveis
3. Clique em **"🔧 Otimizar Agora"**
4. Aguarde a conclusão

## 📊 Estrutura do Projeto

```
FiveMOptimizer/
├── FiveMOptimizer.UI/          # Interface WPF
├── FiveMOptimizer.Core/        # Lógica principal
├── FiveMOptimizer.Services/    # Serviços
├── FiveMOptimizer.Helpers/     # Helpers e utilidades
├── FiveMOptimizer.Models/      # Modelos de dados
└── FiveMOptimizer.sln          # Solução Visual Studio
```

## 🔐 Segurança

- ✅ O aplicativo solicita privilégios de administrador
- ✅ Cria logs de todas as operações
- ✅ Suporta ponto de restauração do Windows
- ✅ Código aberto e verificável

## 📝 Logs

Os logs são salvos em:
```
%APPDATA%\FiveMOptimizer\Logs\log_YYYY-MM-DD.txt
```

## 🤝 Contribuindo

Contribuições são bem-vindas! Por favor:

1. Faça um Fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## ⚠️ Disclaimer

Este aplicativo foi desenvolvido para otimizar sistemas Windows. Use por sua conta e risco. O autor não se responsabiliza por danos causados pelo uso indevido.

**Recomendações:**
- Crie um ponto de restauração antes de usar
- Faça backup de seus dados importantes
- Use em um horário em que não estiver usando aplicativos críticos

## 📄 Licença

Este projeto está sob licença MIT. Veja o arquivo LICENSE para detalhes.

## 🐛 Reportar Bugs

Encontrou um bug? Abra uma **Issue** descrevendo:
- O que aconteceu
- Como reproduzir
- Seu sistema operacional
- Versão do .NET instalada

## 💬 Suporte

Para dúvidas, abra uma **Discussão** no GitHub ou crie uma **Issue**.

## 🎯 Roadmap

- [ ] Otimização de startup
- [ ] Gerenciador de drivers
- [ ] Scheduler automático
- [ ] Interface Dark Mode
- [ ] Suporte a múltiplos idiomas

---

**Desenvolvido com ❤️ para a comunidade FiveM**
