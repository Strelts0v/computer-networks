package com.gv.coding;

import com.gv.App;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.math.BigInteger;
import java.util.ArrayList;
import java.util.List;
import java.util.Random;

public class HammingCoder implements Coder {

    private final Logger logger = LoggerFactory.getLogger(App.class);

    private Random random = new Random(System.currentTimeMillis());

    private static final int RADIX = 2;
    private static final int BIT_COUNT_IN_SYMBOL = 8;

    @Override
    public String encode(String source) {
        String sourceBits = getSourceBits(source);

        int m = sourceBits.length();
        int r = calculateParityBitCount(m);
        logger.info("Number of parity bits needed : " + r);

        int encodeBitsSize = m + r;
        int encodeMessageInBits[] = prepareControlBits(sourceBits, encodeBitsSize);

        logger.info("Encode message in bits with 0 parity bits");
        logSourceBits(encodeMessageInBits, encodeBitsSize);

        encodeMessageInBits = calculateParityBits(r, encodeBitsSize, encodeMessageInBits);

        //Display encoded message
        logger.info("Hamming Encoded Message : ");
        logSourceBits(encodeMessageInBits, encodeBitsSize);

        StringBuilder encodeMessageBuff = new StringBuilder();
        for(int i = 1; i <= encodeBitsSize; i++) {
            encodeMessageBuff.append(encodeMessageInBits[i]);
        }
        return encodeMessageBuff.toString();
    }

    @Override
    public String decode(String encodeSource) {
        int[] encodeSourceBits = getEncodeSourceBits(encodeSource);
        encodeSourceBits = removeParityBits(encodeSourceBits);
        return encodeSourceBitsToString(encodeSourceBits);
    }

    @Override
    public String makeMistakeInEncodeSource(String encodeSource) {
        int index = random.nextInt(encodeSource.length());
        char originalBit = encodeSource.charAt(index);
        char mistakeBit = getMistakeBit(originalBit);
        StringBuilder sb = new StringBuilder(encodeSource);
        sb.setCharAt(index, mistakeBit);
        String encodeSourceWithMistake = sb.toString();
        logger.info("Source after mistake in index " + index +  ":\n" + encodeSourceWithMistake);
        return encodeSourceWithMistake;
    }

    private char getMistakeBit(char bit){
        char mistakeBit = '0';
        switch (bit){
            case '0':
                mistakeBit = '1'; break;
            case '1':
                mistakeBit = '0'; break;
        }
        return mistakeBit;
    }

    @Override
    public String checkMistake(String encodeSource) {
        int[] encodeSourceBits = getEncodeSourceBits(encodeSource);
        encodeSourceBits = removeParityBits(encodeSourceBits);

        int m = encodeSourceBits.length;
        int r = calculateParityBitCount(m);
        int encodeBitsSize = m + r;
        int encodeMessageInBits[] = prepareControlBits(convertIntArrayToString(encodeSourceBits), encodeBitsSize);

        encodeMessageInBits = calculateParityBits(r, encodeBitsSize, encodeMessageInBits);
        // convert it to String
        StringBuilder encodeSourceBuff = new StringBuilder();
        for(int i = 1; i <= encodeBitsSize; i++) {
            encodeSourceBuff.append(encodeMessageInBits[i]);
        }
        String resultEncodeSource = encodeSourceBuff.toString();

        // search and correct mistake
        return correctMistake(resultEncodeSource, encodeSource);
    }

    private String correctMistake(String resultEncodeSource, String encodeSource){
        int parityBitsSum = getParityBitsSum(resultEncodeSource, encodeSource);
        StringBuilder result = new StringBuilder(encodeSource);
        result.setCharAt(parityBitsSum - 1, getMistakeBit(encodeSource.charAt(parityBitsSum - 1)));

        logger.info("Encode source after correcting mistake:\n" + result.toString());
        return result.toString();
    }

    private int getParityBitsSum(String resultEncodeSource, String encodeSource){
        int currentPowerOfTwo = 0;
        int currentPositionOfControlBit;
        int parityBitSum = 0;

        // put information bits on their positions and left 0 control bits positions
        for(int i = 1; i < resultEncodeSource.length(); i++){
            currentPositionOfControlBit = (int) Math.pow(RADIX, currentPowerOfTwo);
            if(i % currentPositionOfControlBit == 0) {
                if(resultEncodeSource.charAt(i - 1) != encodeSource.charAt(i - 1)){
                    parityBitSum += i;
                }
            }
            else {
                currentPowerOfTwo++;
            }
        }
        logger.info("Parity bit sum: " + parityBitSum);
        return parityBitSum;
    }

    private int[] getEncodeSourceBits(String encodeSource){
        int[] encodeSourceBits = new int[encodeSource.length()];
        for(int i = 0; i < encodeSource.length(); i++){
            int bit = Integer.parseInt(String.valueOf(encodeSource.charAt(i)));
            encodeSourceBits[i] = bit;
        }
        return encodeSourceBits;
    }

    private String encodeSourceBitsToString(int[] encodeSourceBits){
        StringBuilder source = new StringBuilder();
        int symbolCount = encodeSourceBits.length / BIT_COUNT_IN_SYMBOL + 1;
        int i = 0;
        while(symbolCount > 0) {
            StringBuilder symbol = new StringBuilder();
            for (int j = 0; j < BIT_COUNT_IN_SYMBOL - 1; j++) {
                symbol.append(encodeSourceBits[i++]);
            }
            int charCode = Integer.parseInt(symbol.toString(), RADIX);
            String str = Character.toString((char) charCode);
            source.append(str);
            symbolCount--;
            i++;
        }
        return source.toString();
    }

    private int[] removeParityBits(int[] encodeSourceBits){
        int currentPowerOfTwo = 0;
        int currentPositionOfControlBit;

        List<Integer> infoBits = new ArrayList<>(encodeSourceBits.length);
        // put information bits on their positions and left 0 control bits positions
        for(int i = 1; i <= encodeSourceBits.length; i++){
            currentPositionOfControlBit = (int) Math.pow(RADIX, currentPowerOfTwo);
            if(i % currentPositionOfControlBit != 0) {
                infoBits.add(encodeSourceBits[i - 1]);
            }
            else {
                currentPowerOfTwo++;
            }
        }
        return infoBits.stream().mapToInt(i->i).toArray();
    }

    private String getSourceBits(String source){
        return new BigInteger(source.getBytes()).toString(RADIX);
    }

    private int calculateParityBitCount(int m){
        int r = 0;
        //calculate number of parity bits needed using (m + r + 1 <= 2 ^ r)
        while(true){
            if(m + r + 1 <= Math.pow(RADIX, r)){
                break;
            }
            r++;
        }
        // to get valid count of parity bits decrement it
        return r;
    }

    private int[] prepareControlBits(String sourceBits, int encodeBitsSize){
        int currentPowerOfTwo = 0;
        int currentPositionOfControlBit;
        int j = 0;
        int encodeMessageInBits[] = new int[encodeBitsSize + 1]; // + 1 because starts with 1 (for convenience)

        // put information bits on their positions and left 0 control bits positions
        for(int i = 1; i <= encodeBitsSize; i++){
            currentPositionOfControlBit = (int) Math.pow(RADIX, currentPowerOfTwo);
            if(i % currentPositionOfControlBit != 0) {
                encodeMessageInBits[i] = Integer.parseInt(Character.toString(sourceBits.charAt(j)));
                j++;
            }
            else {
                currentPowerOfTwo++;
            }
        }
        return encodeMessageInBits;
    }

    private int[] calculateParityBits(int r, int encodeBitsSize, int[] encodeMessageInBits){
        for(int i = 0; i < r; i++) {
            int smallStep = (int )Math.pow(RADIX, i);
            int bigStep = smallStep * RADIX;
            int start = smallStep,checkPos=start;
            logger.info("Calculating Parity bit for Position : " + smallStep);
            logger.info("Bits to be checked : ");
            while(true) {
                for(int k = start; k <= start + smallStep - 1; k++) {
                    checkPos=k;
                    System.out.print(checkPos + " ");
                    if(k > encodeBitsSize) {
                        break;
                    }
                    encodeMessageInBits[smallStep]^=encodeMessageInBits[checkPos];
                }
                if(checkPos > encodeBitsSize) {
                    break;
                }
                else {
                    start = start + bigStep;
                }
            }
        }
        return encodeMessageInBits;
    }


    private void logSourceBits(int[]encodeMessageInBits, int encodeBitsSize){
        System.out.println();
        for(int i = 1; i <= encodeBitsSize; i++) {
            System.out.print(encodeMessageInBits[i]);
        }
        System.out.println();
    }

    private String convertIntArrayToString(int[] intArray){
        StringBuilder sb = new StringBuilder();
        for(int i = 0; i < intArray.length; i++){
            sb.append(intArray[i]);
        }
        return sb.toString();
    }
}
