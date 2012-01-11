require "../lib/unspell"
require "test/unit"

#class LetterTree
#  attr_accessor :tree, :badWord
#end
 
class UnspellerTests < Test::Unit::TestCase
  def test_change_no_vowel_word
    actual = Unspell.change_vowels("blrg")
    assert_equal("blrg", actual)
  end
  
  def test_change_all_vowels
    actual = Unspell.change_vowels("eiouya", true)
    
    assert_equal("aeiouy", actual)
  end
  
  def test_change_capital_vowel
    actual = Unspell.change_vowels("E", true)
    
    assert_equal("A", actual)
  end
  
  def test_change_vowels_in_word
    actual = Unspell.change_vowels("sheep", true)
    
    assert_equal("shaep", actual)
  end
  
  def test_get_repeating_chars
    actual = Unspell.repeat_characters("job", true)
    
    assert_equal("joobbb", actual)
  end
  
  def test_change_capitalization_on_word
    actual = Unspell.change_capitalization("yUp", true)
    
    assert_equal("YuP", actual)
  end
  
  def test_unspell_word
    actual = Unspell.unspell_word("yUp", true)
    
    assert_equal("AeePPP", actual)
  end
end
