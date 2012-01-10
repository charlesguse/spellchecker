class String
  @@Vowels = ['a', 'e', 'i', 'o', 'u', 'y']
  
  def match_case(character_to_match)
    if character_to_match == character_to_match.upcase
      return self.upcase
    else
      self.downcase
    end
  end
end