require "../lib/string_extension"
require "test/unit"
 
class LetterTreeSpellcheckTests < Test::Unit::TestCase
  def test_vowels_are_in_an_array
    expected = ['a', 'e', 'i', 'o', 'u', 'y']
    
    assert_equal(expected, String.Vowels)
  end
  
  def test_match_case_matches_to_upper
    actual = "aaa".match_case("A")
    
    assert_equal("AAA", actual)
  end
  
  def test_match_case_matches_to_upper_does_nothing_if_giving_uppecase_string
    actual = "AAA".match_case("A")
    
    assert_equal("AAA", actual)
  end
  
    def test_match_case_matches_to_lower
    actual = "AAA".match_case("a")
    
    assert_equal("aaa", actual)
  end
  
  def test_match_case_matches_to_upper_does_nothing_if_giving_uppecase_string
    actual = "aaa".match_case("a")
    
    assert_equal("aaa", actual)
  end
  
  def test_match_case_matches_to_upper_with_string_of_mixed_case
    actual = "aAa".match_case("B")
    
    assert_equal("AAA", actual)
  end
  
  def test_match_case_matches_to_lower_with_string_of_mixed_case
    actual = "aAa".match_case("b")
    
    assert_equal("aaa", actual)
  end
  
  def test_match_case_returns_nil_if_string_passed_in_is_mixed_case
    actual = "aAa".match_case("aB")
    
    assert_equal(nil, actual)
  end
  
  def test_character_is_vowel
    actual = "a".is_vowel?
    
    assert_equal(true, actual)
  end
  
  def test_character_is_not_vowel
    actual = "b".is_vowel?
    
    assert_equal(false, actual)
  end
  
  def test_second_character_is_vowel
    actual = "ba".is_vowel?(1)
    
    assert_equal(true, actual)
  end
  
  def test_second_character_is_not_vowel
    actual = "ab".is_vowel?(1)
    
    assert_equal(false, actual)
  end
  
  def test_return_false_if_index_is_out_of_range
    actual = "a".is_vowel?(1)
    
    assert_equal(false, actual)
  end
end