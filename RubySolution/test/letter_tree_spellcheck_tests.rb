require "../lib/letter_tree"
require "test/unit"

class LetterTree
  attr_accessor :tree, :badWord
end


 
class LetterTreeSpellcheckTests < Test::Unit::TestCase
  def setup
    @words = LetterTree.new;
  end
  
  def test_spellecheck_no_suggestion_word
    actual = @words.spellcheck("ohaithar")
    
    assert_equal("NO SUGGESTION", actual)
  end
  
  def test_spellcheck_properly_spelled_word
    @words.add("hi")
    @words.add("noise")
    @words.add("morenoise")
    @words.add("even more noise")
    @words.add("hihihihihi")
    
    actual = @words.spellcheck("hi")
    assert_equal("hi", actual)
  end
end