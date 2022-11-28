# Banner command in GForth

Simple banner command rewrite in GForth

```
  ok
s" BANNER" banner 
 #####     ##    ##  ##  ##  ##  ######  #####                                  
 ##  ##   ####   ### ##  ### ##  ##      ##  ##                                 
 ##  ##  ##  ##  ######  ######  ##      ##  ##                                 
 #####   ######  ######  ######  ####    #####                                  
 ##  ##  ##  ##  ## ###  ## ###  ##      ####                                   
 ##  ##  ##  ##  ##  ##  ##  ##  ##      ## ##                                  
 #####   ##  ##  ##  ##  ##  ##  ######  ##  ##                                 
                                                                                
 ok

```

As a bonus, there is also a scroll in terminal included in the code, that you can run with the following command:

```
initbuf   ok
40 to scroll-speed   ok
s" GFORTH RULEZ!!           " scroll
##  ##  ##      ######  ######    ##      ##
##  ##  ##      ##          ##    ##      ##
##  ##  ##      ##         ##     ##      ##
##  ##  ##      ####      ##      ##      ##
##  ##  ##      ##       ##
##  ##  ##      ##      ##
 ####   ######  ######  ######    ##      ##

 ok
```
