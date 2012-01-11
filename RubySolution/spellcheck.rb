#!/usr/bin/env ruby
require "./lib/letter_tree"
require "./lib/shared"

def print_usage()
  puts("Welcome to Charlie's Amazing Spellchecker!!")
  puts("")
  puts("Usage:")
  puts("")
  puts("spellcheck <optional file name>")
  puts("If you want to call it without a filename, put the dictionary")
  puts("file in this directory (#{File.absolute_path(File.dirname(__FILE__))}) and name it words")
  puts("or make sure a file named words exists in \"/usr/share/dict/\"")
end

if __FILE__ == $0 
  words = LetterTree.new
  
  path = nil
  if ARGV.length != 0
    arg = ARGV[0].strip
    if arg == "help" || arg == "--help" || arg == "/?" || arg == "?"
      print_usage()
      exit
    else
      path = arg
    end
  end
  
  if Shared.populate_words(words, path)
    line = ""
    while line != nil
      print "> "
      STDOUT.flush
      line = $stdin.gets()
      if line != nil
        line.strip!
        puts(words.spellcheck(line))
      end
    end
  else
    print_usage()
  end  
end
