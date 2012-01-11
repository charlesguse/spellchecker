#!/usr/bin/env ruby
require "./lib/unspell"
require "./lib/shared"

def are_args_correct?()
  if ARGV.length != 1 && ARGV.length != 2
    return false
  end
  
  begin
    Integer(ARGV[0].strip)
  rescue
    return false
  end
  
  return true
end

def print_usage()
  puts("Welcome to Charlie's Amazing Unspeller!!");
  puts("");
  puts("Usage:");
  puts("");
  puts("unspell <# of times to run> <optional file name>");
  puts("If you want to call it without a filename, put the dictionary");
  puts("file in this directory (#{File.absolute_path(File.dirname(__FILE__))}) and name it words")
  puts("or make sure a file named words exists in \"/usr/share/dict/\"")
end

if __FILE__ == $0 
  
  if !are_args_correct?
    print_usage()
    exit
  end
  
  words = []
  times_to_run = Integer(ARGV[0])
  path = nil
  if ARGV.length == 2
    path = ARGV[1]
  end
  
  if Shared.populate_words(words, path) && words.length > 0
    while times_to_run > 0
      word_to_unspell = Unspell.unspell_word(words[rand(words.length)])
      puts(word_to_unspell)
      times_to_run -= 1
    end
  else
    print_usage()
  end  
end
