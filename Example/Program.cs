// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");

// List<(Instruction instruction, Dictionary<float, List<(Delegate del, object[] args)>> threadedCode)> bytecode;

List<BytecodeInstruction> bytecode;

public record BytecodeInstruction(Instruction Instruction, Dictionary<float, ThreadedFunction> Functions);

public record ThreadedFunction(Delegate Function, object[] Args);

// There are no default values! All values should be issued by InstructionEnumManager
public enum Instruction
{
    Invalid,
}

public static class InstructionEnumManager
{
    private static int _curInstructionNumber = (int)Instruction.Invalid + 1;
    public static Instruction GetNextInstruction() => (Instruction)_curInstructionNumber++;
}