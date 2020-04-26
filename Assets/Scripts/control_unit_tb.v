`timescale 1ns / 1ps

module alu_control_tb;

    // Inputs
    reg clk, reset, alu_op, instruction_5_0;

    // Outputs
    wire alu_out;

    // Instantiate Unit Under Test (uut)
    my_alu uut(
            .clk(clk), .reset(reset), .alu_op(), .instruction_5_0(instruction_5_0)
    );

    initial begin 
        // Initialize inputs
        clk = 0; // Clear clock
        reset = 0; // Clear reset

        // Wait 100 ns for global reset to finish
        #100;

        // Synchronous reset
        reset = 1; clk = 0; #5
        reset = 1; clk = 1; #5
        reset = 0; clk = 0; #5

        // Start the clock
        forever begin 
            #5 clk = ~clk;
        end
    end

    integer totalTests = 0;
    integer failedTests = 0;
    initial begin
        // Initialize inputs

        // Wait 100 ns for global reset to finish
        #100;

        // Wait for reset to finish
        #10

        // Test 1:
        $write("Test case 1: alu_up = 00 & instr5_0 = 000000 ( function: sw/lw )\n");
        totalTests = totalTests + 1;
        // Set input values
        alu_op = 2'b00 instruction_5_0 = 6'b000000; #100;
        // Check if result != expected
        if ( alu_out != 4'b0010 ) begin 
            $display("...failed");
            failedtests += 1;
        end else begin
            $display("...passed");
        end
        #10

        // Test 2:
        $write("Test case 2: alu_up = 01 & instr5_0 = 000000 ( function : beq)\n");
        totalTests = totalTests + 1;
        // Set input values
        alu_op = 2'b01 instruction_5_0 = 6'b000000; #100;
        // Check if result != expected
        if ( alu_out != 4'b0110 ) begin 
            $display("...failed");
            failedtests += 1;
        end else begin
            $display("...passed");
        end
        #10

        // Test 3:
        $write("Test case 3: alu_up = 10 & instr5_0 = 100000 ( function: add )\n");
        totalTests = totalTests + 1;
        // Set input values
        alu_op = 2'b10 instruction_5_0 = 6'b100000; #100;
        // Check if result != expected
        if ( alu_out != 4'b0010 ) begin 
            $display("...failed");
            failedtests += 1;
        end else begin
            $display("...passed");
        end
        #10

        // Test 4:
        $write("Test case 4: alu_up = 10 & instr5_0 = 100010 ( function: sub )\n");
        totalTests = totalTests + 1;
        // Set input values
        alu_op = 2'b10 instruction_5_0 = 6'b100010; #100;
        // Check if result != expected
        if ( alu_out != 4'b0110 ) begin 
            $display("...failed");
            failedtests += 1;
        end else begin
            $display("...passed");
        end
        #10

        // Test 5:
        $write("Test case 5: alu_up = 10 & instr5_0 = 100100 ( function: and )\n");
        totalTests = totalTests + 1;
        // Set input values
        alu_op = 2'b10 instruction_5_0 = 6'b100100; #100;
        // Check if result != expected
        if ( alu_out != 4'b0000 ) begin 
            $display("...failed");
            failedtests += 1;
        end else begin
            $display("...passed");
        end
        #10

        // Test 6:
        $write("Test case 6: alu_up = 10 & instr5_0 = 100101 ( function: or )\n");
        totalTests = totalTests + 1;
        // Set input values
        alu_op = 2'b10 instruction_5_0 = 6'b100101; #100;
        // Check if result != expected
        if ( alu_out != 4'b0001 ) begin 
            $display("...failed");
            failedtests += 1;
        end else begin
            $display("...passed");
        end
        #10

        // Test 7:
        $write("Test case 7: alu_up = 10 & instr5_0 = 100111 ( function: NOR )\n");
        totalTests = totalTests + 1;
        // Set input values
        alu_op = 2'b10 instruction_5_0 = 6'b100111; #100;
        // Check if result != expected
        if ( alu_out != 4'b1100 ) begin 
            $display("...failed");
            failedtests += 1;
        end else begin
            $display("...passed");
        end
        #10

        // Test 8:
        $write("Test case 8: alu_up = 10 & instr5_0 = 101010 ( function: set on less than )\n");
        totalTests = totalTests + 1;
        // Set input values
        alu_op = 2'b10 instruction_5_0 = 6'b101010; #100;
        // Check if result != expected
        if ( alu_out != 4'b0111 ) begin 
            $display("...failed");
            failedtests += 1;
        end else begin
            $display("...passed");
        end
        #10

        $display("\n-----------------------------------------");
        $display("\nTesting complete\nPassed %d/%d tests", totalTests-failedTests,totalTests);
        $display("\n-----------------------------------------");
    end
endmodule
