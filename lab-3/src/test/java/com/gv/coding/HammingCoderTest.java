package com.gv.coding;

import org.junit.Assert;
import org.junit.Test;

public class HammingCoderTest {

    private Coder coder = new HammingCoder();

    @Test
    public void encodeShouldReturnValidStrOfBits() throws Exception {
        final String source = "h"; //1101000
        final String expectedEncodeSource = "10101010000";
        final String errorMessage = "Expected different bytes in encode source";

        final String resultEncodeSource = coder.encode(source);
        Assert.assertEquals(errorMessage, expectedEncodeSource, resultEncodeSource);
    }

    @Test
    public void decodeShouldReturnValidStrFromBits() throws Exception {
        final String encodeSource = "10101010000";
        final String expectedSource = "h";
        final String errorMessage = "Expected different message after decoding of encodeSource";

        final String resultSource = coder.decode(encodeSource);
        Assert.assertEquals(errorMessage, expectedSource, resultSource);
    }
}