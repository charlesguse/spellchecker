class String
  @@Vowels = ['a', 'e', 'i', 'o', 'u', 'y']
  
  def self.Vowels
    return @@Vowels
  end
  
  def match_case(character_to_match)
    if character_to_match == character_to_match.upcase
      return self.upcase
    elsif character_to_match == character_to_match.downcase
      return self.downcase
    end
  end
  
  def is_vowel?(index=0)
    char_to_check = self[index]
    @@Vowels.each do |vowel|
      if char_to_check.downcase == vowel
        return true
      end
    end if char_to_check != nil
    return false
  end
end
