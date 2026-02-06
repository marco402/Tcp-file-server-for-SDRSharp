##
-Once a security key is set up, the executable is transferred to release  
              (button on the main repository window).  
-Version change: version 0.0.0.5.  

## Transmit wav or raw IQ files to SDRSharp source RTL-SDR TCP.  
    Use:  
        Select wav files (unprocessed RF64) and/or Raw IQ (e.g. CU8 from RTL_433).  
            -All with the same sampling rate.  
            -Choose:  
                -Port.  
                -Delay between each file.  
                -Number of consecutive issues for each file.  
                -Number of issues of all files.  
    For SDRSharp:  
        Source=RTL-SDR TCP.  
        -Choose.  
            -Host.  
            -Port.  
            -Sample rate (WARNING: host and port are stored but not the sample rate).  
    Start Server.  
    Start or restart SDRSharp.  
    
## Concatene Wav  
    - Concatenation of the selected wav files.  
        -Files can be separated by a delay, Attention 1ms at 250k=1000Bytes.  
        -Select files with the same sample rate otherwise sample rate of the first one.  
        -The output file is in the location of the selected files.  
        -It is called: concateneWav_+sampleRate+k+date.wav.  
        -Files with a different sampling rate than the first one are rejected. Raw IQ Concatenate.  
        
## Concatenation of raw IQ Files.  
    -Concatenation of selected IQ files.  
        -Files can be separated by a delay, Attention 1ms at 250k=250Bytes if 8 bits.  
        -Select files with the same sample rate or sample rate of the first one.  
        -The output file is in the location of the selected files.  
        -His name is: concateneCU8_+sampleRate+k+date.wav.  
        -Files with a different sample rate than the first one are rejected.  
_____________________________________________________________________________________________________  