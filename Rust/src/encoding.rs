use indexmap::IndexMap;

fn calculate_checksum(data: &[u8]) -> Result<u8, &'static str> {
    if data.len() != 5 {
        return Err("Block header did not pass its own sum");
    }
    let mut checksum = data[0];
    for i in 1..data.len() {
        checksum ^= data[i];
    }
    Ok(checksum)
}

fn encode_message_block(block_type: &str, data: &str) -> Result<Vec<u8>, &'static str> {
    let header = block_type.as_bytes();
    let data_bytes = data.as_bytes();
    let length_byte = vec![data_bytes.len() as u8];
    let mut header_checker = Vec::with_capacity(header.len() + length_byte.len());

    header_checker.extend_from_slice(header);
    header_checker.extend_from_slice(&length_byte);

    let checksum = match calculate_checksum(&header_checker) {
        Ok(checksum) => checksum,
        Err(err_msg) => return Err(err_msg),
    };

    if checksum != calculate_checksum(&header_checker)? {
        return Err("Block header checksum failed");
    }

    let mut block = Vec::with_capacity(header.len() + length_byte.len() + data_bytes.len() + 1);

    block.extend_from_slice(header);
    block.extend_from_slice(&length_byte);
    block.push(checksum);
    block.extend_from_slice(data_bytes);

    Ok(block)
}

fn encode_message(data: &IndexMap<&str, &str>) -> Vec<u8> {
    let mut message = Vec::new();
    let mut discard_blocks = false;

    for (key, value) in data.iter() {
        if !discard_blocks {
            match encode_message_block(key, value) {
                Ok(message_block) => message.extend_from_slice(&message_block),
                Err(_) => {
                    println!("Block header error");
                    message.clear();
                    discard_blocks = true;
                }
            }
        }
    }
    return message;
}

pub fn encoding_data() {
    let mut input1 = IndexMap::new();
    input1.insert("sndr", "ewater");
    input1.insert("rcvr", "foo,works");
    input1.insert("sens", "heart,beat");
    input1.insert("time", "2023,08,16T13:07");

    let mut input2 = IndexMap::new();
    input2.insert("sndr", "ewater");
    input2.insert("rcvr", "foo,works");
    input2.insert("sens", "temperature");
    input2.insert("data", "15");

    let mut input3 = IndexMap::new();
    input3.insert("sndr", "ewater");
    input3.insert("rcvr", "foo,works");
    input3.insert("sens", "body,temperature");
    input3.insert("dat", "36");

    let message_bytes1 = encode_message(&input1);
    println!("Input 1 Message Bytes: {:?}", message_bytes1);

    let message_bytes2 = encode_message(&input2);
    println!("Input 2 Message Bytes: {:?}", message_bytes2);

    // instead of panic, it just returns an error
    let message_bytes3 = encode_message(&input3);
    println!("Input 3 Message Bytes: {:?}", message_bytes3);
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_input1_encoding() {
        // arrange
        let expected_value = vec![
            115, 110, 100, 114, 6, 13, 101, 119, 97, 116, 101, 114, 114, 99, 118, 114, 9, 28, 102,
            111, 111, 44, 119, 111, 114, 107, 115, 115, 101, 110, 115, 10, 1, 104, 101, 97, 114,
            116, 44, 98, 101, 97, 116, 116, 105, 109, 101, 16, 5, 50, 48, 50, 51, 44, 48, 56, 44,
            49, 54, 84, 49, 51, 58, 48, 55,
        ];
        let mut input = IndexMap::new();
        input.insert("sndr", "ewater");
        input.insert("rcvr", "foo,works");
        input.insert("sens", "heart,beat");
        input.insert("time", "2023,08,16T13:07");

        // act
        let message_bytes = encode_message(&input);

        // assert
        assert_eq!(message_bytes.len(), expected_value.len());
        assert_eq!(message_bytes, expected_value);
    }

    #[test]
    fn test_get_data_input() {
        // arrange
        let expected_value = vec![
            115, 110, 100, 114, 6, 13, 101, 119, 97, 116, 101, 114, 114, 99, 118, 114, 9, 28, 102,
            111, 111, 44, 119, 111, 114, 107, 115, 115, 101, 110, 115, 11, 0, 116, 101, 109, 112,
            101, 114, 97, 116, 117, 114, 101, 100, 97, 116, 97, 2, 18, 49, 53,
        ];
        let mut input = IndexMap::new();
        input.insert("sndr", "ewater");
        input.insert("rcvr", "foo,works");
        input.insert("sens", "temperature");
        input.insert("data", "15");

        //act
        let message_bytes = encode_message(&input);

        //assert
        assert_eq!(message_bytes.len(), expected_value.len());
        assert_eq!(message_bytes, expected_value);
    }

    #[test]
    fn test_inut_with_wrong_key() {
        // arrange
        let expected_value = [];
        let mut input = IndexMap::new();
        input.insert("sndr", "ewater");
        input.insert("rcvr", "foo,works");
        input.insert("sens", "body,temperature");
        input.insert("dat", "36");

        //act
        let message_bytes = encode_message(&input);

        //assert
        assert_eq!(message_bytes.len(), expected_value.len());
        assert_eq!(message_bytes, expected_value);
    }
}
