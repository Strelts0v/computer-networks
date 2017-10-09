package com.gv.coding;

public interface Coder {

    String encode(String source);

    String decode(String encodeSource);

    String makeMistakeInEncodeSource(String encodeSource);

    String checkMistake(String encodeSource);
}
