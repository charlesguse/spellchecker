require "#{File.dirname(__FILE__)}/traversal_data"
require "#{File.dirname(__FILE__)}/letter_node"
require "#{File.dirname(__FILE__)}/string_extension"
require "set"

#Dir[File.dirname(__FILE__) + '/lib/*.rb'].each {|file| require file }

class LetterTree
  @@NO_SUGGESTION_TEXT = "NO SUGGESTION"
  
  # Create the object
  def initialize()
    @tree = {}
    @bad_words = {}
    @root = self.get_root()
  end
  
  def get_root()
    treeData = TraversalData.new
    treeData.depth = -1
    treeData.current_node = nil
    treeData.nodes = @tree
    return treeData
  end
  
  def add(word)
    traversal = self.traverse(word, self.get_root())
    self.add_at_location(word, traversal)
  end
  
  def add_at_location(word, traversal)    
    if traversal.depth < word.length - 1
      traversal.depth += 1
      node = LetterNode.new
      node.parent = traversal.current_node
      node.letter = word[traversal.depth]
      traversal.nodes[node.letter] = node
      
      if traversal.depth + 1 < word.length
        traversal.current_node = node
        traversal.nodes = node.nodes
        add_at_location(word, traversal)
      else
        node.end = true
      end
    else
      traversal.current_node.end = true
    end
  end
  
  def traverse(word, traversal)
    begin
      oldSpot = traversal.depth
      traversal = self.traverse_one_step(word, traversal)
    end while traversal.depth != oldSpot
    
    return traversal
  end
  
  def traverse_one_step(word, traversal_ref)
    traversal = traversal_ref.clone
    current_node = self.traverse_node(word, traversal)

    if current_node != nil
      traversal.current_node = current_node
      traversal.nodes = current_node.nodes
      traversal.depth += 1
    end
    
    return traversal
  end
  
  def traverse_node(word, traversal)
    if word.length > 0 && traversal.depth < word.length - 1
      potential_key = word[traversal.depth+1]
      
      if traversal.nodes.has_key?(potential_key)
        return traversal.nodes[potential_key]
      end
    end
  end
  
  def spellcheck(word)
    root = self.get_root()
    new_word = self.recursive_spellcheck(word, root)
    @bad_words.clear()
    return new_word
  end
  
  def recursive_spellcheck(word, traversal, changing_vowel = false,
                           changing_case = false)
    current_depth = traversal.depth
    # If the word is not in the badword list, or if the depth is not in the set of depth
    # checked for the bad word, run the spellcheck.
    if !@bad_words.has_key?(word) || !@bad_words[word].include?(current_depth)
      if traversal.current_node != nil
        current_word = traversal.current_node.get_word()
        
        if traversal.current_node.end == true && current_word == word
          return current_word
        end
      end
      
      next_location = self.traverse_one_step(word, traversal)
      if current_depth != next_location.depth
        returned_word = self.recursive_spellcheck(word, next_location)
        
        if returned_word != @@NO_SUGGESTION_TEXT
          return returned_word
        end
      end
      
      # If a vowel was chanaged to cause the repeating character,
      # don't remove the repeated character.
      if (current_depth + 1 < word.length && word[current_depth+1].is_vowel?) \
         || !changing_vowel
        returned_word = self.process_repeated_letter(word, traversal)
        
        if returned_word != @@NO_SUGGESTION_TEXT
          return returned_word
        end
      end
      
      if !changing_vowel
        returned_word = self.check_for_incorrect_vowel(word, traversal, changing_case)
        
        if returned_word != @@NO_SUGGESTION_TEXT
          return returned_word
        end
      end
      
      if !changing_case
        returned_word = self.check_for_improper_casing(word, traversal, changing_vowel)
        
        if returned_word != @@NO_SUGGESTION_TEXT
          return returned_word
        end
      end
      
      if !@bad_words.has_key?(word)
        @bad_words[word] = Set.new
      end
      @bad_words[word].add(current_depth)
    end
    return @@NO_SUGGESTION_TEXT
  end
  
  def check_for_improper_casing(word, traversal, changing_vowel)
    depth_to_check = traversal.depth + 1
    
    if 0 <= depth_to_check && depth_to_check < word.length
      new_word = word.clone
      new_word[depth_to_check] = new_word[depth_to_check].swapcase
      
      return self.recursive_spellcheck(new_word, traversal, changing_vowel, true)
    end
    return @@NO_SUGGESTION_TEXT
  end
  
  def check_for_incorrect_vowel(word, traversal, changing_case)
    depth_to_check = traversal.depth + 1
    
    if 0 <= depth_to_check && depth_to_check < word.length \
       && word[depth_to_check].is_vowel?
      new_word = word.clone
      # check each vowel but don't run spellcheck again on the passed in vowel
      String.Vowels.each do |vowel|
        if vowel != word[depth_to_check].downcase
          new_word[depth_to_check] = vowel.match_case(new_word[depth_to_check])
          
          returned_word = self.recursive_spellcheck(new_word, traversal, true, changing_case)
          
          if returned_word != @@NO_SUGGESTION_TEXT
            return returned_word
          end
        end
      end
    end
    return @@NO_SUGGESTION_TEXT
  end
  
  def process_repeated_letter(word, traversal)
    new_word = word.clone
    while traversal.depth + 2 < new_word.length \
          && new_word[traversal.depth + 1] == new_word[traversal.depth + 2]
      new_word.slice!(traversal.depth + 1)
      
      if traversal.current_node != nil
        new_traversal = traversal.clone
        new_traversal = self.traverse_one_step(new_word, new_traversal)
        new_traversal = self.traverse_one_step(new_word, new_traversal)
        
        if traversal.depth + 2 == new_traversal.depth && new_traversal.current_node.end \
           && new_traversal.current_node.get_word() == new_word
          return new_traversal.current_node.get_word()
        end
      end
      returned_word = self.recursive_spellcheck(new_word, traversal)
          
      if returned_word != @@NO_SUGGESTION_TEXT
        return returned_word
      end
    end 
    return @@NO_SUGGESTION_TEXT
  end
end
