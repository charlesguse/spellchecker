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
  
  def test_spellcheck_properly_spelled_one_letter_word
    @words.add("a")
    @words.add("hi")
    @words.add("noise")
    @words.add("morenoise")
    @words.add("even more noise")
    @words.add("hihihihihi")
    
    actual = @words.spellcheck("a")
    assert_equal("a", actual)
  end
  
  def test_spellcheck_mismatched_case
    @words.add("inside")
    
    actual = @words.spellcheck("inSIDE")
    
    assert_equal("inside", actual)
  end
  
  def test_spellcheck_incorrect_vowel
    @words.add("wake")
    
    actual = @words.spellcheck("weke")
    
    assert_equal("wake", actual)
  end
  
  def test_spellcheck_repeated_letters
    @words.add("job")
    
    actual = @words.spellcheck("jjoobbb")
    
    assert_equal("job", actual)
  end
  
  def test_spellcheck_examples_on_website
    @words.add("sheep")
    @words.add("people")
    @words.add("inside")
    @words.add("job")
    @words.add("wake")
    @words.add("conspiracy")
    
    assert_equal("sheep", @words.spellcheck("sheeeeep"))
    assert_equal("people", @words.spellcheck("peepple"))
    assert_equal("NO SUGGESTION", @words.spellcheck("sheeple"))
    assert_equal("inside", @words.spellcheck("inSIDE"))
    assert_equal("job", @words.spellcheck("jjoobbb"))
    assert_equal("conspiracy", @words.spellcheck("CUNsperrICY"))
  end
  
  def test_spellcheck_words_that_need_backtracking_to_find
    @words.add("wake")
    @words.add("west")
    
    actual = @words.spellcheck("weke")
    
    assert_equal("wake", actual)
  end
  
  def test_spellcheck_words_that_have_multiple_casings
    # Actual example from my word list.
    # That is what made me realize that this is not handled properly.
    @words.add("Wake")
    @words.add("wake")
    
    actual = @words.spellcheck("Wake")
    assert_equal("Wake", actual)
    
    actual = @words.spellcheck("wake")
    assert_equal("wake", actual)
  end
  
  def test_spellcheck_words_that_have_repeating_characters_with_multiple_casings
    @words.add("Azov's")
    
    actual = @words.spellcheck("OozZAavv''S")
    
    assert_equal("Azov's", actual)
  end
  
  def test_spellecheck_words_that_have_repeating_vowels_next_to_other_vowels
    @words.add("Beaumont")
    
    actual = @words.spellcheck("bOoaaOOMuUnTt")
    
    assert_equal("Beaumont", actual)
  end
end
