using System;

namespace GuessNumberGame
{
    // Принцип единственной ответственности (Single Responsibility)
    public interface INumberGenerator
    {
        int Generate(int min, int max);
    }

    public class RandomNumberGenerator : INumberGenerator
    {
        private readonly Random _random = new Random();

        public int Generate(int min, int max)
        {
            return _random.Next(min, max + 1);
        }
    }

    // Принцип открытости/закрытости (Open/Closed)
    public interface IGameSettings
    {
        int MinNumber { get; }
        int MaxNumber { get; }
        int MaxAttempts { get; }
    }

    public class DefaultGameSettings : IGameSettings
    {
        public int MinNumber => 1;
        public int MaxNumber => 100;
        public int MaxAttempts => 10;
    }

    // Принцип разделения интерфейса (Interface Segregation)
    public interface IUserInput
    {
        int GetGuess();
    }

    public interface IUserOutput
    {
        void DisplayMessage(string message);
        void DisplayHint(string hint);
    }

    public class ConsoleUserInput : IUserInput
    {
        public int GetGuess()
        {
            Console.Write("Введите вашу догадку: ");
            return int.Parse(Console.ReadLine());
        }
    }

    public class ConsoleUserOutput : IUserOutput
    {
        public void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void DisplayHint(string hint)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(hint);
            Console.ResetColor();
        }
    }

    // Принцип подстановки Лисков (Liskov Substitution)
    public abstract class Game
    {
        protected readonly IUserInput _userInput;
        protected readonly IUserOutput _userOutput;
        protected readonly IGameSettings _settings;
        protected readonly INumberGenerator _numberGenerator;

        protected Game(IUserInput userInput, IUserOutput userOutput,
                      IGameSettings settings, INumberGenerator numberGenerator)
        {
            _userInput = userInput;
            _userOutput = userOutput;
            _settings = settings;
            _numberGenerator = numberGenerator;
        }

        public abstract void Play();
    }

    public class GuessNumberGame : Game
    {
        private int _targetNumber;
        private int _attemptsLeft;

        public GuessNumberGame(IUserInput userInput, IUserOutput userOutput,
                             IGameSettings settings, INumberGenerator numberGenerator)
            : base(userInput, userOutput, settings, numberGenerator)
        {
        }

        // Принцип инверсии зависимостей (Dependency Inversion)
        public override void Play()
        {
            InitializeGame();
            _userOutput.DisplayMessage($"Угадайте число от {_settings.MinNumber} до {_settings.MaxNumber}. У вас {_settings.MaxAttempts} попыток.");

            while (_attemptsLeft > 0)
            {
                try
                {
                    int guess = _userInput.GetGuess();
                    _attemptsLeft--;

                    if (guess == _targetNumber)
                    {
                        _userOutput.DisplayMessage($"Поздравляем! Вы угадали число {_targetNumber} за {_settings.MaxAttempts - _attemptsLeft} попыток.");
                        return;
                    }

                    string hint = guess < _targetNumber ? "Загаданное число больше." : "Загаданное число меньше.";
                    _userOutput.DisplayHint(hint);
                    _userOutput.DisplayMessage($"Осталось попыток: {_attemptsLeft}");
                }
                catch (FormatException)
                {
                    _userOutput.DisplayMessage("Пожалуйста, введите целое число.");
                }
            }

            _userOutput.DisplayMessage($"К сожалению, вы не угадали. Загаданное число было: {_targetNumber}");
        }

        private void InitializeGame()
        {
            _targetNumber = _numberGenerator.Generate(_settings.MinNumber, _settings.MaxNumber);
            _attemptsLeft = _settings.MaxAttempts;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Композиция корня приложения
            var settings = new DefaultGameSettings();
            var numberGenerator = new RandomNumberGenerator();
            var input = new ConsoleUserInput();
            var output = new ConsoleUserOutput();

            var game = new GuessNumberGame(input, output, settings, numberGenerator);
            game.Play();
        }
    }
}
