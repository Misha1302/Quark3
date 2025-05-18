### Innovative approach to bytecode

New bytecode arranged like
`List<(Instruction instruction, Dictionary<float, List<(Delegate del, object[] args)>> threadedCode)> bytecode`:

```
List<BytecodeInstruction> bytecode;

public record BytecodeInstruction(Instruction Instruction, Dictionary<float, ThreadedFunction> Functions);

public record ThreadedFunction(Delegate Function, object[] Args);
```

The main idea is the generalization of bytecode. E.g., we haven't `setLoc` instruction now, but we have generalized
`loadRef` and `set`. E.g., basic bytecode was:

```
push 5
setLoc i
```

Now it will be like:

```
push [{ 0, { PushConst, [5] }}]             // Stack: 5(int)
loadRef [{ 0, { LoadLocalRef, ["i"] }}]     // Stack: 5(int), i(IVariable)
set [{ 0, { SetIVariable, [] }}]            // Stack empty
```

We do not load reference of local, we do not use concrete instruction to load reference to local. "Extensions" should
analyze bytecode's instructions and add their functions for each necessary instruction. You can notice, `loadRef` loads
`IVariable` instance. It's not a concrete type 'cause we need to save maximum generalization. E.g., you can add
structures, and they will implement `IVariable` to so as not to write new code and new instructions.

`IVariable` will something be like this:

```
public interface IVariable
{
    public void SetValue(IAny value);
    public IAny GetValue();
}
```

### Improved approach to instructions enum

Now only one value by default in `Instruction` enum: Invalid. All others should be got and saved via
`InstructionEnumManager`. Something like this:

```
public enum Instruction { Invalid }

public static class InstructionEnumManager
{
    private static int _curInstructionNumber = (int)Instruction.Invalid + 1;
    public static Instruction GetNextInstruction() => (Instruction)_curInstructionNumber++;
}

class SetInstruction
{
    ...
    public static Instruction Set = InstructionEnumManager.GetNextInstruction()
    ...
}
```