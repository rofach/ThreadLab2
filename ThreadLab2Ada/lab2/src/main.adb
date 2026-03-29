with Ada.Integer_Text_IO;
with Ada.Real_Time;
with Ada.Text_IO; use Ada.Text_IO;

procedure main is
   use type Ada.Real_Time.Time;
   arr_size : positive := 1_000_000_000;
   task main_task is
      pragma Storage_Size (arr_size * (Integer'Size / 8));
   end main_task;
   task body main_task is
      length : constant positive := arr_size;
      negative_index : constant positive := length / 2;
      negative_value : constant integer := -1_000;
      type int_array is array (1 .. length) of integer;
      worker_count : positive;
      data : int_array;   
      type thread_count_array is array (Positive range <>) of Positive;
      thread_counts : constant thread_count_array := (1, 2, 4, 8, 12);

      protected result_manager is
         procedure submit_min (val : in integer);
         entry get_final_min (val : out integer);
         procedure reset;
      private
         current_min : integer := integer'last;
         tasks_count : integer := 0;
      end result_manager;

      protected body result_manager is
         procedure submit_min (val : in integer) is
         begin
            if val < current_min then
               current_min := val;
            end if;
            tasks_count := tasks_count + 1;
         end submit_min;

         entry get_final_min (val : out integer) when tasks_count = worker_count is
         begin
            val := current_min;
         end get_final_min;

         procedure reset is
         begin
            current_min := integer'last;
            tasks_count := 0;
         end reset;
      end result_manager;

      procedure initialize_array is
      begin
         for i in data'range loop
            data (i) := integer ((i mod 10_000) + 1);
         end loop;
         data (negative_index) := negative_value;
      end initialize_array;

      task type find_min_task is
         entry start (start_index, finish_index : in positive);
      end find_min_task;

      task body find_min_task is
         first, last : positive;
         local_min : integer := integer'last;
      begin
         accept start (start_index, finish_index : in positive) do
            first := start_index;
            last := finish_index;
         end start;

         for i in first .. last loop
            if data (i) < local_min then
               local_min := data (i);
            end if;
         end loop;

         result_manager.submit_min (local_min);
      end find_min_task;
   begin
      put_line ("initializing array...");
      initialize_array;
      for c in thread_counts'range loop
         worker_count := thread_counts (c);
         Put_Line ("running with " & worker_count'img & " threads");
         result_manager.reset;
         declare
            type finder_array is array (positive range <>) of find_min_task;
            threads : finder_array (1 .. worker_count);
            
            first_index, last_index : positive;
            final_result : integer;
            start_time, finish_time : Ada.Real_Time.Time;
            segment_length : constant positive := length / worker_count;
         begin
            put_line ("starting threads");
            start_time := Ada.Real_Time.Clock;
            for i in threads'range loop
               first_index := segment_length * (i - 1) + 1;
               last_index := segment_length * i;
               
               if i = worker_count then
                  last_index := length;
               end if;
               threads (i).start (first_index, last_index);
            end loop;

            result_manager.get_final_min (final_result);

            finish_time := Ada.Real_Time.Clock;

            put_line ("minimum found:" & final_result'img);
            put_line ("time:" & 
               duration'image(Ada.Real_Time.To_Duration(finish_time - start_time)) & 
               " seconds");
         end;
      end loop;
   end main_task;
begin
   null;
end main;