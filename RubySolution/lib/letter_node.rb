
class LetterNode
  attr_accessor :letter, :parent, :nodes, :end
  
  def initialize()
    @end = false
    @nodes = {}
  end
  
  def clone()
    new_node = LetterNode.new
    new_node.letter = @letter.clone
    new_node.parent = @parent.clone
    new_node.nodes = @nodes.clone
    new_node.end = @end.clone
    return new_node
  end
  
  def get_word()
    word = String.new

    traverser = self
      
    begin
      word.insert(0, traverser.letter)
      traverser = traverser.parent
    end while traverser != nil
    
    return word
  end
end
