require "#{File.dirname(__FILE__)}/string_extension"

class Unspell
  @@MAX_TIMES_TO_REPEAT_CHAR = 2
  
  def self.change_vowels(word_ref, test_mode=false)
    word = word_ref.clone
    vowel_to_use = -1
    changing_vowel = false
    for i in 0..word.length - 1
      if word[i].is_vowel?
        if test_mode
          vowel_to_use += 1
          vowel_to_use %= String.Vowels.length
          changing_vowel = true
        else
          changing_vowel = rand(2) == 0
          vowel_to_use = rand(String.Vowels.length)
        end
        
        # change the current vowel to the new vowel and match the previous case
        if changing_vowel
          word[i] = String.Vowels[vowel_to_use].match_case(word[i])
        end
      end
    end
    return word
  end
  
  def self.repeat_characters(word_ref, test_mode=false)
    word = word_ref.clone
    times_to_repeat = word.length
    
    (word.length - 1).downto(0) do |i|
      if test_mode
        times_to_repeat -= 1
      else
        times_to_repeat = rand(@@MAX_TIMES_TO_REPEAT_CHAR)
      end
      
      char_to_repeat = word[i]
      
      for j in 1..times_to_repeat
        word.insert(i, char_to_repeat)
      end
    end
    return word
  end
  
  def self.change_capitalization(word_ref, test_mode=false)
    word = word_ref.clone
    changing_case = false
    for i in 0..word.length - 1
      if test_mode
        changing_case = true
      else
        changing_case = rand(2) == 0
      end
      
      if changing_case
        word[i] = word[i].swapcase
      end
    end
    return word
  end
  
  def self.unspell_word(word_ref, test_mode=false)
    word = word_ref.clone
    word = Unspell.change_vowels(word, test_mode)
    word = Unspell.repeat_characters(word, test_mode)
    word = Unspell.change_capitalization(word, test_mode)
    return word
  end
end
