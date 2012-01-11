class Shared
  @@POTENTIAL_PATHS = ["words", "/usr/share/dict/words"]
  
  def self.populate_words(words, path)
    if path != nil
      return Shared.populate_words_with_specific_path(words, path)
    else
      populated = false
      @@POTENTIAL_PATHS.each do |potential_path|
        populated = Shared.populate_words_with_specific_path(words, potential_path)
        if populated == true
          break
        end
      end
      return populated
    end
  end

  def self.populate_words_with_specific_path(words, path)
    begin
      file = File.open(path)
      file.each_line do |line|
        if line.strip!.length > 0
          if words.respond_to?(:add)
            words.add(line)
          elsif words.respond_to?(:push)
            words.push(line)
          end
        end
      end
    #rescue
    #  return false
    ensure
      file.close unless file == nil
    end
    return true
  end
end
