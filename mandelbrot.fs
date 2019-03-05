(
    Some utils for float stack first
)

\ Pop float value from local stack
: l>f ( F: -- f ) f@local0 lp+ ;

\ Opposite direction of frot (float rotate)
: -frot ( a b c - c a b ) frot frot ;


(
    Barebones complex number system
)

\ Complex value is size of two floats
: complex ( -- n ) float float + ;
: complex+ ( z-addr1 -- z-addr2 ) float+ float+ ;

\ Create complex value into dictionary
: zvariable create complex allot ;

: z! ( F: z -- ) fswap dup f! float+ f! ;
: z@ ( F: -- z ) dup f@ float+ f@ ;

: z.  ( F: z -- ) fswap ." (" f. ." , " f. ." )" ;

\ Stack operations for complex values
: zdup ( F: z -- z z ) fover fover ;
: zdrop ( F: z1 -- ) fdrop fdrop ;
: zover ( F: z1 z2 -- z1 z2 z1 ) 3 fpick 3 fpick ;
: zswap ( F: z1 z2 -- z2 z1 ) f>l -frot l>f -frot ;

: re ( F: z -- f ) fdrop ;
: im ( F: z -- f ) fnip ;

\ Comparing floating point values is difficult. We take shortcut.
: z=  ( F: z1 z2 -- ? ) frot f= f= and ;

: z+  ( F: z1 z2 -- z )
    \ r1 i1 r2 i2
    frot f+
    \ r1 r2 i3
    frot frot f+
    \ i3 r3
    fswap
    \ r3 i3
;

\  Complex multiplication
\
\  See: http://mathworld.wolfram.com/ComplexMultiplication.html
\
\    (a + ib) * (c + id)
\    = ac + aid + ibc + ibid
\    = ac + i*(ad + bc) - bd
\    = Re\{ ac-bd }, Im\{ (a+b)(c+d) - ac - bd }

: z*  ( F: z1 z2 -- z )
    \ a b c d  -- calculate ac
    fover 4 fpick f* f>l
    \ a b c d  -- calculate bd
    fdup 3 fpick f* f>l
    \ a b c d -- calculate (a+b)(c+d)
    f+ frot frot f+ f*
    \ (a+b)(c+d)
    f@local1 f- f@local0 f-
    \ (a+b)(c+d) - ac - bd == i1
    l>f l>f fswap f-
    \ i1 r1
    fswap
;

: zabs ( F: z1 -- f ) fdup f* fswap fdup f* f+ fsqrt ;

(
    The Mandelbrot
)

\ Mandelbrot's function: f(z) = z^2 + c
: mandelbrot-function ( F: z1 z2 -- z ) zswap zdup z* z+ ;

\ Mandelbrot's function is bounded if its length is 2 or below
: bounded? ( -- ? ) ( F: z -- ) zdup zabs 2e0 f<= ;

(
    Iterate Mandelbrot
)
: mandelbrot ( n -- n ) ( F: z1 -- )
    0        \ return value
    0e0 0e0  \ F: initial value of z
             \ F: cr ci zr zi (f(z) = z^2 + c)
             \ stack: n 0
    begin
        2dup >    \ While our iteration depth (n) is not reached
        bounded?  \ and mandelbrot's function is bounded
        and       \ we keep iterating
    while
        zover mandelbrot-function
        1+
    repeat
    zdrop zdrop \ No need for our f(z) any more
    nip
;

(
    Mandelbrot image from -2 + i1 to 1 - i1
)
2.92968750000E-3 fconstant X-STEP
2.60416666667E-3 fconstant Y-STEP

: generate-mandelbrot ( -- )
    ." P2" cr
    ." 1024 768" cr
    ." 10" cr

    -2e0 1e0    ( upper left corner of image )
    768 0 do
        1024 0 do
            zdup 10 mandelbrot
            X-STEP 0e0 z+
            .
        loop
        -2e0 fswap
        0e0 Y-STEP fnegate z+
        cr
    loop
;
