require "../lib/letter_tree"
require "test/unit"

class LetterTree
  attr_accessor :tree, :badWord
end

class LetterTreePopulationTests < Test::Unit::TestCase
  
  def setup
    @words = LetterTree.new;
  end
  
  def test_populate_no_words_in_tree
    assert_instance_of(Hash, @words.tree)
    assert_equal(0, @words.tree.length)
  end
  
  def test_populate_single_word_in_tree
    @words.add("hi")
    
    assert_equal(1, @words.tree.length)
    assert_equal(1, @words.tree['h'].nodes.length)
    assert_equal(0, @words.tree['h'].nodes['i'].nodes.length)
    
    assert_equal('h', @words.tree['h'].letter)
    assert_equal('i', @words.tree['h'].nodes['i'].letter)
    
    assert_equal(false, @words.tree['h'].end)
    assert_equal(true, @words.tree['h'].nodes['i'].end)
    
    assert_equal("hi", @words.tree['h'].nodes['i'].get_word())
  end
  
  def test_populate_non_overlapping_words_in_tree
    @words.add("hi")
    @words.add("a")
    
    assert_equal(2, @words.tree.length)
    assert_equal(1, @words.tree['h'].nodes.length)
    assert_equal(0, @words.tree['a'].nodes.length)
    
    assert_equal('h', @words.tree['h'].letter)
    assert_equal('i', @words.tree['h'].nodes['i'].letter)
    
    assert_equal('a', @words.tree['a'].letter)
    
    assert_equal(false, @words.tree['h'].end)
    assert_equal(true, @words.tree['h'].nodes['i'].end)
    
    assert_equal(true, @words.tree['a'].end)
  end
  
  def test_populate_same_word_in_tree_twice
    @words.add("a")
    @words.add("a")
    
    assert_equal(1, @words.tree.length)
  end
  
  def test_populate_overlapping_words_in_tree
    @words.add("hi")
    @words.add("ha")
    
    assert_equal(1, @words.tree.length)
    assert_equal(2, @words.tree['h'].nodes.length)
    
    assert_equal('h', @words.tree['h'].letter)
    assert_equal('i', @words.tree['h'].nodes['i'].letter)
    assert_equal('a', @words.tree['h'].nodes['a'].letter)
    
    assert_equal(false, @words.tree['h'].end)
    assert_equal(true, @words.tree['h'].nodes['i'].end)
    assert_equal(true, @words.tree['h'].nodes['a'].end)
  end
  
  def test_populate_smaller_word_using_same_characters_as_bigger_word
    @words.add("high")
    @words.add("hi")
    
    assert_equal(1, @words.tree.length)
    assert_equal(1, @words.tree['h'].nodes.length)
    assert_equal(1, @words.tree['h'].nodes['i'].nodes.length)
    assert_equal(1, @words.tree['h'].nodes['i'].nodes['g'].nodes.length)
    assert_equal(0, @words.tree['h'].nodes['i'].nodes['g'].nodes['h'].nodes.length)
    
    assert_equal('h', @words.tree['h'].letter)
    assert_equal('i', @words.tree['h'].nodes['i'].letter)
    assert_equal('g', @words.tree['h'].nodes['i'].nodes['g'].letter)
    assert_equal('h', @words.tree['h'].nodes['i'].nodes['g'].nodes['h'].letter)
    
    assert_equal(false, @words.tree['h'].end)
    assert_equal(true, @words.tree['h'].nodes['i'].end)
    assert_equal(false, @words.tree['h'].nodes['i'].nodes['g'].end)
    assert_equal(true, @words.tree['h'].nodes['i'].nodes['g'].nodes['h'].end)
  end
end
