#!/bin/sh
cmd="/app/imageproxy"

if ! [ -z ${IMAGEPROXY_PORT+x} ]; then
    cmd="$cmd $IMAGEPROXY_PORT"
else
    cmd="$cmd 8187"
fi

if ! [ -z ${IMAGEPROXY_PROXY+x} ]; then
    cmd="$cmd $IMAGEPROXY_PROXY"
fi

$cmd
