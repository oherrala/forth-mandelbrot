#! /usr/bin/env gforth

assert-level 3
require mandelbrot.fs

: complex-tests
    ." Test z=" cr
    assert( 1e0 0e0 1e0 0e0 z= )
    assert( 1e0 1e0 1e0 0e0 z= false = )

    ." Test zdup" cr
    assert( 1e0 2e0 zdup z= )

    ." Test zover" cr
    assert( 1e0 2e0 3e0 4e0 zover zswap zdrop z= )

    ." Test z+" cr
    assert( 1e0 2e0 3e0 4e0 z+ 4e0 6e0 z= )

    ." Test z*" cr
    assert( 1e0 2e0 3e0 4e0 z* -5e0 10e0 z= )

    ." Test zabs" cr
    assert( 2e0 2e0 zabs 2e0 2e0 fsqrt f* f= )

    ." Test re" cr
    assert( 2e0 3e0 re 2e0 f= )

    ." Test im" cr
    assert( 2e0 3e0 im 3e0 f= )
;

: mandelbrot-tests
    ." Test mandelbrot" cr
    assert( 0e0 0e0 1e0 1e0 mandelbrot-function 1e0 1e0 z= )
    assert( 1e0 2e0 0e0 0e0 mandelbrot-function -3e0 4e0 z= )
;

complex-tests mandelbrot-tests

." all done. bye." cr
bye
