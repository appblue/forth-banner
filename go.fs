: empty		s" ---marker--- marker ---marker---" evaluate ;

: edit		s" vim main.fs" system ;
: run		s" main.fs" included ; 
: ecr		edit run ;

marker ---marker---
ecr
