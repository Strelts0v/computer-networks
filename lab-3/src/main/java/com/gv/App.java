package com.gv;

import com.gv.coding.Coder;
import com.gv.coding.HammingCoder;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

public class App {

    private final static Logger logger = LoggerFactory.getLogger(App.class);

    public static void main( String[] args ){
        String source1 = "xqan";
        String source2 = "zrwq";
        String source3 = "zrwq";

        Coder hamming = new HammingCoder();

        hamming.encode(source1);
        logger.info("\n----------------------------\n");
        hamming.encode(source2);
        String encodeSource3 = hamming.encode(source3);
        String decodeSource3 = hamming.decode(encodeSource3);
        String encodeSource3WithMistake = hamming.makeMistakeInEncodeSource(encodeSource3);
        String encodeSource3AfterCorrectingMistake = hamming.checkMistake(encodeSource3WithMistake);

        logger.info("Result:");
        logger.info("Original source: " + source3);
        logger.info("----------------------------\nEncode source:");
        logger.info(encodeSource3);
        logger.info("Decode source:");
        logger.info(decodeSource3);
        logger.info("Encode source with mistake:");
        logger.info(encodeSource3WithMistake);
        logger.info("Encode source after correcting mistake:");
        logger.info(encodeSource3AfterCorrectingMistake);

    }
}
